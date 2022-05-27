using System.Net;
using System.Text.RegularExpressions;

namespace RecTimeLogic
{
    public class Tv4StreamManager : StreamManager
    {
        public Tv4StreamManager(string uri, IStreamDownloader downloader) : base(uri, downloader)
        {
        }

        public override void DownloadAndParseData()
        {
            var url = BaseUrl;
            if (this.BaseUrl.Contains("?playlist"))
                url = BaseUrl.Substring(0, BaseUrl.IndexOf("?playlist"));

            var match = Regex.Match(url, @"^https?:\/\/(?:www\.)?tv4(?:play)?\.se\/.*(?:-|\/)(\d+)");
            if (match.Success)
            {
                Streams.Clear();

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback += (s, certificate, chain, sslPolicyErrors) => true;

                masterUrl = $"https://playback-api.b17g.net/asset/{match.Groups[1]}?service=tv4&device=browser&drm=widevine&protocol=hls%2Cdash";
                var data = streamDownloader.Download(masterUrl);

                var drm = Regex.Match(data, @"""isDrmProtected"":(.+?),");
                if (drm.Success && drm.Groups[1].Value.Trim() == "true")
                    throw new DrmProtectionException("DRM skyddad video! Kan ej laddas ned tyvärr.");

                var title = Regex.Match(data, @"""title"":""(.+?)""");
                if (title.Success)
                    Title = title.Groups[1].Value;

                var imageMatch = Regex.Match(data, @"""image"":""(.+?)""");
                if (imageMatch.Success)
                {
                    PosterUrl = imageMatch.Groups[1].Value;
                    PosterImage = streamDownloader.DownloadImage(PosterUrl);
                }

                var durationMatch = Regex.Match(data, @"""duration"":(\d+)");
                if (durationMatch.Success)
                    Duration = int.Parse(durationMatch.Groups[1].Value);

                int bitrate = 0;
                var bitMatch = Regex.Match(data, @"""ns_st_cl"":""(\d+)""");
                if (bitMatch.Success)
                    bitrate = int.Parse(bitMatch.Groups[1].Value);


                var media = Regex.Match(data, @"""mediaUri"":""(.+?)""");
                if (media.Success)
                {
                    var mediaUrl = "https://playback-api.b17g.net" + media.Groups[1].Value;
                    data = streamDownloader.Download(mediaUrl);

                    var manifest = Regex.Match(data, @"""manifestUrl"":""(.+?)""");
                    if (manifest.Success)
                    {
                        if (manifest.Groups[1].Value.Contains(".m3u8"))
                        {
                            masterUrl = manifest.Groups[1].Value;
                            Streams.Clear();
                            ParseStreams(streamDownloader.Download(masterUrl));
                        }
                        else
                        {
                            var stream = new StreamInfo()
                            {
                                Url = manifest.Groups[1].Value,
                                Bandwidth = bitrate,
                                ApproxSize = (Duration * (bitrate / 1024) / 1024 / 8)
                            };

                            var type = Regex.Match(data, @"""type"":""(.+?)""");
                            if (type.Success)
                                stream.Resolution = type.Groups[1].Value;

                            Streams.Add(stream);
                        }

                    }
                }
            }
        }
    }
}
