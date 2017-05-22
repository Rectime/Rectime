using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RecTimeLogic
{
    public class UrStreamManager : StreamManager
    {
        private string _urBaseurl = @"http://streaming3.ur.se/";

        public UrStreamManager(string uri, IStreamDownloader downloader) : base(uri, downloader)
        {
            
        }

        public override void DownloadAndParseData()
        {
            //base.DownloadAndParseData();

            var html = streamDownloader.Download(BaseUrl);

            var titleMatch = Regex.Match(html, @"""title"":""(.+?)""");
            if (titleMatch.Success)
                Title = UnEscapeString(titleMatch.Groups[1].Value);

            var imageMatch = Regex.Match(html, @"""image"":""(.+?)""");
            if (imageMatch.Success)
            {
                PosterUrl = UnEscapeString(imageMatch.Groups[1].Value);
                PosterUrl = PosterUrl.StartsWith("//") ? "https:" + PosterUrl : PosterUrl;
                PosterImage = streamDownloader.DownloadImage(PosterUrl);
            }

            var hlsMatch = Regex.Match(html, @"""hls_file"":""(.+?)""");
            var playlist = string.Empty;
            if (hlsMatch.Success)
                playlist = UnEscapeString(hlsMatch.Groups[1].Value);

            var durationMatch = Regex.Match(html, @"""duration"":(\d+)");
            if (durationMatch.Success)
                Duration = int.Parse(durationMatch.Groups[1].Value);

            var streamHdMatch = Regex.Match(html, @"""file_http_hd"":""(.+?)""");
            if (streamHdMatch.Success)
            {
                var streamUrl = _urBaseurl + UnEscapeString(streamHdMatch.Groups[1].Value) + playlist;
                ParseStreams(streamDownloader.Download(streamUrl), UnEscapeString(streamHdMatch.Groups[1].Value), "");
            }

            var streamHdSubMatch = Regex.Match(html, @"""file_http_sub_hd"":""(.+?)""");
            if (streamHdSubMatch.Success)
            {
                var streamUrl = _urBaseurl + UnEscapeString(streamHdSubMatch.Groups[1].Value) + playlist;
                ParseStreams(streamDownloader.Download(streamUrl), UnEscapeString(streamHdSubMatch.Groups[1].Value), "TEXT");
            }
        }

        private string UnEscapeString(string input)
        {
            input = input.Replace(@"\\", @"\");
            input = input.Replace(@"\/", @"/");
            Regex rx = new Regex(@"\\[uU]([0-9A-Fa-f]{4})");
            input = rx.Replace(input, match => ((char)Int32.Parse(match.Value.Substring(2), NumberStyles.HexNumber)).ToString());
            return input;
        }

        private void ParseStreams(string data, string streamBaseUrl, string extra)
        {
            if (string.IsNullOrEmpty(data))
                return;

            var lines = new List<string>(data.Trim().Split('\n'));

            if (lines.Count < 4)
                return;

            lines.RemoveRange(0, 2);

            for (int i = 0; i < lines.Count / 2; i++)
            {
                string line1 = lines[i * 2];
                string line2 = lines[i * 2 + 1];

                var matchLine1 = Regex.Match(line1, "BANDWIDTH=([0-9]+),CODECS=\"(.+)\",RESOLUTION=([0-9x]+)");
                var matchLine2 = Regex.Match(line2, "(.+)\\?");

                if (matchLine1.Success && matchLine2.Success)
                {
                    var bandwidth = int.Parse(matchLine1.Groups[1].Value);
                    var info = new StreamInfo()
                    {
                        Url = _urBaseurl + streamBaseUrl + matchLine2.Groups[1].Value,
                        Bandwidth = bandwidth,
                        Resolution = matchLine1.Groups[3].Value,
                        Codec = matchLine1.Groups[2].Value,
                        ApproxSize = (Duration * (bandwidth / 1024) / 1024 / 8),
                        Extra = extra
                    };
                    Streams.Add(info);
                }
            }
        }
    }
}
