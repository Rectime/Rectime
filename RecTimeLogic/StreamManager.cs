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
        public string LongTitle => !string.IsNullOrEmpty(Title) ? Title : ProgramTitle + "-" + Title;
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
            else
            {
                var tryAgain = Regex.Match(data, @"data-video-id=""(.+?)""");
                if (tryAgain.Success)
                    videoId = tryAgain.Groups[1].Value;
            }

            var titleMatch = Regex.Match(data, @"<title data-react-helmet=""true"">(.+?)<");
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

            if(Type == SourceType.ÖppetArkiv || Type == SourceType.Svt)
                data = streamDownloader.Download("https://api.svt.se/videoplayer-api/video/" + videoId);
            else
                data = streamDownloader.Download("https://api.svt.se/video/" + videoId);

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

            //Separate audio?
            string audioUrl = null;

            var audiomatch = Regex.Match(data, "#EXT-X-MEDIA:TYPE=AUDIO,URI=\"(?<audio>[^\"]+)\"");
            if (audiomatch.Success)
            {
                audioUrl = audiomatch.Groups["audio"].Value;
            }

            var lines = new List<string>(data.Trim().Split('\n'));
            //lines.RemoveAt(0);

            for (int i = 0; i < lines.Count - 1; i++)
            {
                string line1 = lines[i];
                string line2 = lines[i + 1];

                var matchLine1 = Regex.Match(line1, "BANDWIDTH=(?<band>[0-9]+),RESOLUTION=(?<res>[0-9x]+),CODECS=\"(?<codec>.+)\"");
                var matchLine2 = Regex.Match(line2, "^[^#](.+)m3u8");

                // Try again, new pattern
                if (!matchLine1.Success)
                {
                    matchLine1 = Regex.Match(line1, "BANDWIDTH=(?<band>[0-9]+),CODECS=\"(?<codec>.+)\",RESOLUTION=(?<res>[0-9x]+)");
                }

                if (matchLine1.Success && matchLine2.Success)
                {
                    var bandwidth = int.Parse(matchLine1.Groups["band"].Value);
                    var info = new StreamInfo()
                    {
                        Url = (matchLine2.Groups[0].Value.ToLower().StartsWith("http")) 
                            ? matchLine2.Groups[0].Value : 
                            UrlHelper.GetBaseMasterUrl(masterUrl) + matchLine2.Groups[0].Value,
                        Bandwidth = bandwidth,
                        Resolution = matchLine1.Groups["res"].Value,
                        Codec = matchLine1.Groups["codec"].Value,
                        ApproxSize = (Duration * (bandwidth / 1024) / 1024 / 8),
                        StreamType = (string.IsNullOrEmpty(audioUrl)) ? StreamType.VideoAndAudio : StreamType.VideoSeparateAudio,
                        AudioUrl = (audioUrl != null && audioUrl.ToLower().StartsWith("http")) ? audioUrl : UrlHelper.GetBaseMasterUrl(masterUrl) + audioUrl
                    };
                    Streams.Add(info);
                }
            }
        }
    }
}
