using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RecTimeLogic
{
    public class YouTubeStreamManager : StreamManager
    {
        private const string VideoUrlsSeparator = ",";

        public YouTubeStreamManager(string url, IStreamDownloader streamDownloader) : base(url, streamDownloader)
        {
        }

        public override void DownloadAndParseData()
        {
            string info = streamDownloader.Download(DataUrl);
            var infoColletion = HttpUtility.ParseQueryString(info);

            Title = infoColletion["title"];
            if (string.IsNullOrEmpty(Title))
                Title = HttpUtility.ParseQueryString(new Uri(BaseUrl).Query)["v"];

            Duration = string.IsNullOrEmpty(infoColletion["length_seconds"]) ? 0 : int.Parse(infoColletion["length_seconds"]);

            var availableFormats = infoColletion["url_encoded_fmt_stream_map"];
            if (availableFormats == string.Empty)
                return; 

            var formatList = new List<string>(Regex.Split(availableFormats, VideoUrlsSeparator));

            //formatList.ForEach(format =>
            //{
            //    if (string.IsNullOrEmpty(format.Trim()))
            //        return; 

            //    var formatInfoCollection = HttpUtility.ParseQueryString(format);
            //    var urlEncoded = formatInfoCollection["url"];
            //    var itag = formatInfoCollection["itag"];
            //    var quality = formatInfoCollection["quality"];
            //    var signature = formatInfoCollection["sig"];
            //    var fallbackHost = formatInfoCollection["fallback_host"];
            //    var formatCode = int.Parse(itag);

            //    urlEncoded = string.Format("{0}&fallback_host={1}&signature={2}", urlEncoded, fallbackHost, signature);
            //    var url = new Uri(HttpUtility.UrlDecode(HttpUtility.UrlDecode(urlEncoded)));
            //    Streams.Add(new YouTubeStreamInfo(url.ToString(), quality, formatCode));
            //});

            var hdFormats = infoColletion["adaptive_fmts"];
            formatList = new List<string>(Regex.Split(hdFormats, VideoUrlsSeparator));
            formatList.ForEach(format =>
            {
                if (string.IsNullOrEmpty(format.Trim()))
                    return;

                var formatInfoCollection = HttpUtility.ParseQueryString(format);
                var urlEncoded = formatInfoCollection["url"];
                var itag = formatInfoCollection["itag"];
                var quality = formatInfoCollection["quality_label"];
                var signature = formatInfoCollection["sig"];
                var bitrate = formatInfoCollection["bitrate"];
                var bandwidth = string.IsNullOrEmpty(bitrate) ? 0 : int.Parse(bitrate);
                var fallbackHost = formatInfoCollection["fallback_host"];
                var formatCode = int.Parse(itag);

                urlEncoded = $"{urlEncoded}&fallback_host={fallbackHost}&signature={signature}";
                var url = new Uri(HttpUtility.UrlDecode(HttpUtility.UrlDecode(urlEncoded)));
                var stream = new YouTubeStreamInfo(url.ToString(), quality, formatCode)
                {
                    Bandwidth = bandwidth,
                    ApproxSize = (Duration * (bandwidth / 1024) / 1024 / 8)
                };

                Streams.Add(stream);
                Debug.WriteLine("Found stream id={0} quality={1}", stream.FormatCode, stream.Quality);
            });

            var bestMpaAudio = Streams.Cast<YouTubeStreamInfo>()
                .Where(s => s.FormatCode >= 139 && s.FormatCode <= 141)
                .OrderByDescending(s => s.FormatCode).FirstOrDefault();
            var bestWebMAudio = Streams.Cast<YouTubeStreamInfo>()
                .Where(s => s.FormatCode >= 170 && s.FormatCode <= 171)
                .OrderByDescending(s => s.FormatCode).FirstOrDefault();

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

            Streams.Reverse();
            Streams.RemoveAll(s => s.Resolution.Contains("Unknown"));

            PosterUrl = infoColletion["thumbnail_url"];
            if (!string.IsNullOrEmpty(PosterUrl))
                PosterImage = streamDownloader.DownloadImage(PosterUrl);


        }
    }
}
