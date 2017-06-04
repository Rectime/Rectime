using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web;

namespace RecTimeLogic
{
    public class StreamManager
    {
        public List<StreamInfo> Streams { get; private set; }
        public string DataUrl { get; protected set; }
        public string ProgramTitle { get; private set; }
        public string Title { get; protected set; }
        public int Duration { get; protected set; }
        public string PosterUrl { get; protected set; }
        public Image PosterImage { get; protected  set; }
        public SourceType Type { get; protected set; }
        public string LongTitle => string.IsNullOrEmpty(ProgramTitle) ? Title : ProgramTitle + "-" + Title;
        public string BaseUrl { get; private set; }

        public string GetValidFileName(StreamInfo streamInfo)
        {
            string fileName = LongTitle + streamInfo.FileEnding;
            fileName = HttpUtility.HtmlDecode(fileName);

            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        protected string masterUrl;
        protected readonly IStreamDownloader streamDownloader;

        public StreamManager(string uri, IStreamDownloader downloader)
        {
            this.streamDownloader = downloader;
            Streams = new List<StreamInfo>();

            this.BaseUrl = uri;

            if (!string.IsNullOrEmpty(uri))
            {
                var result = UrlHelper.ParseUrl(uri);
                this.Type = result.Item1;
                this.DataUrl = result.Item2;
            }
        } 

        public virtual void DownloadAndParseData()
        {
            if (string.IsNullOrEmpty(DataUrl))
                return;

            string streamUrl = string.Empty;

            var data = streamDownloader.Download(BaseUrl);

            var videoIdMatch = Regex.Match(data, @"<video data-video-id=""(.+?)""");
            string videoId = "";

            if (videoIdMatch.Success)
                videoId = videoIdMatch.Groups[1].Value;


            var titleMatch = Regex.Match(data, @"<title>(.+?)<");
            if (titleMatch.Success)
            {
                Title = titleMatch.Groups[1].Value;
                if (Title.Contains("|"))
                    Title = Title.Substring(0, Title.IndexOf('|') - 1);
            }

            var imageMatch = Regex.Match(data, @"""thumbnailUrl"" content=""(.+?)""");
            if (imageMatch.Success)
            {
                PosterUrl = imageMatch.Groups[1].Value;
                PosterImage = streamDownloader.DownloadImage(PosterUrl);
            }
            else
            {
                var posterMatch = Regex.Match(data, @"poster=""(.+?)""");
                if (posterMatch.Success)
                {
                    PosterUrl = posterMatch.Groups[1].Value;
                    PosterImage = streamDownloader.DownloadImage(PosterUrl);
                }
            }

            data = streamDownloader.Download("https://api.svt.se/videoplayer-api/video/" + videoId);

            var programTitleMatch = Regex.Match(data, @"""programTitle"":""(.+?)""");
            if (programTitleMatch.Success)
                ProgramTitle = programTitleMatch.Groups[1].Value;

            var durationMatch = Regex.Match(data, @"""contentDuration"":(\d+)");
            if (durationMatch.Success)
                Duration = int.Parse(durationMatch.Groups[1].Value);

            var match = Regex.Match(data, "https:[^\\s:\"\\?]+master\\.m3u8");
            if (match.Success)
            {
                masterUrl = match.Captures[0].Value;
                Streams.Clear();
                ParseStreams(streamDownloader.Download(masterUrl));
            }
        }

        private void ParseStreams(string data)
        {
            if (string.IsNullOrEmpty(data))
                return;

            var lines = new List<string>(data.Trim().Split('\n'));
            lines.RemoveAt(0);

            for (int i = 0; i < lines.Count / 2; i++)
            {
                string line1 = lines[i*2];
                string line2 = lines[i*2 + 1];

                var matchLine1 = Regex.Match(line1, "BANDWIDTH=([0-9]+),RESOLUTION=([0-9x]+),CODECS=\"(.+)\"");
                var matchLine2 = Regex.Match(line2, "(.+)\\?");

                if (matchLine1.Success && matchLine2.Success)
                {
                    var bandwidth = int.Parse(matchLine1.Groups[1].Value);
                    var info = new StreamInfo()
                    {
                        Url = (matchLine2.Groups[1].Value.ToLower().StartsWith("http")) 
                            ? matchLine2.Groups[1].Value : 
                            UrlHelper.GetBaseMasterUrl(masterUrl) + matchLine2.Groups[1].Value,
                        Bandwidth = bandwidth,
                        Resolution = matchLine1.Groups[2].Value,
                        Codec = matchLine1.Groups[3].Value,
                        ApproxSize = (Duration * (bandwidth / 1024) / 1024 / 8)
                    };
                    Streams.Add(info);
                }
            }
        }
    }
}
