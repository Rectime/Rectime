using Newtonsoft.Json.Linq;
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

                //masterUrl = $"https://playback-api.b17g.net/asset/{match.Groups[1]}?service=tv4&device=browser&drm=widevine&protocol=hls%2Cdash";
                masterUrl = $"https://playback2.a2d.tv/play/{match.Groups[1]}?service=tv4&device=browser&browser=GoogleChrome&protocol=hls%2Cdash&drm=widevine&capabilities=live-drm-adstitch-2%2Cexpired_assets";
                var data = streamDownloader.Download(masterUrl);

                var json = JObject.Parse(data);

                JToken title = json.SelectToken("metadata.title", errorWhenNoMatch: false);
                if (title != null)
                    Title = title.ToString();

                JToken drm = json.SelectToken("metadata.isDrmProtected", errorWhenNoMatch: false);
                if (drm != null && drm.ToString().ToLower() == "true")
                    throw new DrmProtectionException("DRM skyddad video! Kan ej laddas ned tyvärr.");

                JToken imageMatch = json.SelectToken("metadata.image", errorWhenNoMatch: false);
                if (imageMatch != null)
                {
                    PosterUrl = imageMatch.ToString();
                    PosterImage = streamDownloader.DownloadImage(PosterUrl);
                }

                JToken durationMatch = json.SelectToken("metadata.duration", errorWhenNoMatch: false);
                if (durationMatch != null)
                    Duration = int.Parse(durationMatch.ToString());

                int bitrate = 0;
                JToken bitMatch = json.SelectToken("trackingData.comScore.ns_st_cl", errorWhenNoMatch: false);
                if (bitMatch != null)
                    bitrate = int.Parse(bitMatch.ToString());


                JToken media = json.SelectToken("playbackItem.manifestUrl", errorWhenNoMatch: false);
                if (media != null)
                {
                    masterUrl = media.ToString();
                    Streams.Clear();
                    ParseStreams(streamDownloader.Download(masterUrl));
                      
                }
            }
        }
    }
}
