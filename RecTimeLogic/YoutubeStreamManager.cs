using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RecTimeLogic
{
    public class YouTubeStreamManager : StreamManager
    {
        private const string VideoUrlsSeparator = ",";
        private const string YouTubeInfoPageUrl = "https://www.youtube.com/get_video_info?&video_id={0}&el=embedded&ps=default&eurl={1}&hl=en";

        public YouTubeStreamManager(string url, IStreamDownloader streamDownloader) : base(url, streamDownloader)
        {
        }

        public override void DownloadAndParseData()
        {
            var id = HttpUtility.ParseQueryString(new Uri(DataUrl).Query)["v"];
            var eurl = WebUtility.HtmlEncode($"https://youtube.googleapis.com/v/{id}");
            DataUrl = string.Format(YouTubeInfoPageUrl, id, eurl);

            string info = streamDownloader.Download(DataUrl);
            var infoColletion = HttpUtility.ParseQueryString(info);

            dynamic json = JObject.Parse(infoColletion["player_response"]);
            Title = json.videoDetails.title;

            foreach (dynamic d in json.streamingData.adaptiveFormats)
            {
                var formatCode = int.Parse(d.itag.ToString());
                var quality = d.quality.ToString();
                var url = new Uri(HttpUtility.UrlDecode(HttpUtility.UrlDecode(d.url.ToString())));
                var stream = new YouTubeStreamInfo(url.ToString(), quality, formatCode);
                int duration = 0;
                try
                {
                    duration = int.Parse(d.approxDurationMs.ToString());
                    Duration = duration;
                }
                catch { duration = Duration; }

                var bitrate = int.Parse(d.bitrate.ToString());
                stream.ApproxSize = (duration / 1000) * (bitrate / 1024) / 1024 / 8;
                stream.Bandwidth = bitrate;
                try
                {
                    stream.Extra = d.fps.ToString() + "fps";
                }
                catch { }

                Streams.Add(stream);
            }
            
            var bestMpaAudio = Streams.Cast<YouTubeStreamInfo>()
                .Where(s => s.Format == "MP4 Audio")
                .OrderByDescending(s => s.Bandwidth).FirstOrDefault();
            var bestWebMAudio = Streams.Cast<YouTubeStreamInfo>()
                .Where(s => s.Format == "WebM Audio")
                .OrderByDescending(s => s.Bandwidth).FirstOrDefault();

            //If we have separate audio streams then select the best for later muxing.
            //If not then hope there is audio in the stream..
            //WebM & Mp4 Video only
            Streams.Cast<YouTubeStreamInfo>().Where(s => s.FileEnding == ".mp4")
                .ToList().ForEach(s =>
            {
                if (bestMpaAudio != null)
                {
                    s.AudioUrl = bestMpaAudio.AudioUrl;
                    s.StreamType = StreamType.VideoSeparateAudio;
                }
            });
            Streams.Cast<YouTubeStreamInfo>().Where(s => s.FileEnding == ".webm")
                .ToList().ForEach(s =>
            {
                if (bestWebMAudio != null)
                {
                    s.AudioUrl = bestWebMAudio.AudioUrl;
                    s.StreamType = StreamType.VideoSeparateAudio;
                }

            });

            Streams.RemoveAll(s => s.Resolution.Contains("Unknown"));
            var ordered = Streams.OrderBy(s => s.Bandwidth).ToList();
            Streams.Clear();
            Streams.AddRange(ordered);

            PosterUrl = json.videoDetails.thumbnail.thumbnails[0].url;
            if (!string.IsNullOrEmpty(PosterUrl))
                PosterImage = streamDownloader.DownloadImage(PosterUrl);
            
        }
    }
}
