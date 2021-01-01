﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

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
            List<StreamInfo> streams = new List<StreamInfo>();

            //base.DownloadAndParseData();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (s, certificate, chain, sslPolicyErrors) => true;

            var html = streamDownloader.Download(BaseUrl);

            var jsonAttr = Regex.Match(html, @"/Player/Player"" data-react-props=""([^\""]+)""");
            if(jsonAttr.Success)
            {
                var jsonText = WebUtility.HtmlDecode(jsonAttr.Groups[1].Value);
                dynamic json = JObject.Parse(jsonText);

                // Title
                Title = json.currentProduct.title;
                var episode = json.currentProduct["episodeNumber"];
                Title = (episode == null) ? Title : Title + " - avsnitt " + episode;

                //Image
                try
                {
                    PosterUrl = json.currentProduct.image["1280x720"];
                    PosterImage = streamDownloader.DownloadImage(PosterUrl);
                }
                catch { }

                //Duration
                try { Duration = int.Parse(json.currentProduct.duration.ToString()); } catch { }

                try
                {
                    var location = json.currentProduct.streamingInfo.raw.sd.location.ToString();
                    var streamUrl = _urBaseurl + location + "playlist.m3u8";
                    ParseStreams(streamDownloader.Download(streamUrl), location, "");
                }
                catch { }

                try
                {
                    var location = json.currentProduct.streamingInfo.raw.hd.location.ToString();
                    var streamUrl = _urBaseurl + location + "playlist.m3u8";
                    ParseStreams(streamDownloader.Download(streamUrl), location, "");
                }
                catch { }


                //subtitle?
                var subtitle = string.Empty;

                try { subtitle = json.currentProduct.streamingInfo.raw.tt.location.ToString(); } catch { }
                try { subtitle = json.currentProduct.streamingInfo.sweComplete.tt.location.ToString(); } catch { }

                if (!string.IsNullOrEmpty(subtitle))
                    foreach (var s in Streams)
                        s.SubtitleUrl = subtitle;
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
