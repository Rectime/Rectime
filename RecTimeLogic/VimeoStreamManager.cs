using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RecTimeLogic
{
    public class VimeoStreamManager : StreamManager
    {
        public VimeoStreamManager(string uri, IStreamDownloader downloader) : base(uri, downloader)
        {
        }

        public override void DownloadAndParseData()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (s, certificate, chain, sslPolicyErrors) => true;

            string data = streamDownloader.Download(DataUrl);

            var cfgUrl = Regex.Match(data, @"data-config-url=""([^""]+)"" data - fallback - url");
            var clipCfgUrl = Regex.Match(data, @"vimeo\.clip_page_config\s*=\s*({.+?});");
            var jsData = Regex.Match(data, "var config = ([^;]+)");
            var jsonData = string.Empty;

            if (cfgUrl.Success)
            {
                var playerUrl = cfgUrl.Groups[1].Value.Replace("&amp;", "&");
                jsonData = streamDownloader.Download(playerUrl);
            }
            else if (clipCfgUrl.Success)
            {
                var pageConfig = clipCfgUrl.Groups[1].Value;
                dynamic json = JObject.Parse(pageConfig);
                var playerUrl = json.player.config_url;
                jsonData = streamDownloader.Download(Convert.ToString(playerUrl));
            }
            else if (jsData.Success)
            {
                jsonData = jsData.Groups[1].Value;
            }

            if (!string.IsNullOrEmpty(jsonData))
            {
                dynamic json = JObject.Parse(jsonData);
                Title = json.video.title;
                Duration = json.video.duration;
                PosterUrl = json.video.thumbs["640"];
                PosterImage = streamDownloader.DownloadImage(PosterUrl);
                var hlsUrl = Convert.ToString(json.request.files.hls.cdns.fastly_skyfire.url);
                var hls = streamDownloader.Download(hlsUrl);
                ParseStreams(hls, hlsUrl);
            }

        }

        private new void ParseStreams(string data, string url)
        {
            if (string.IsNullOrEmpty(data))
                return;

            var lines = new List<string>(data.Trim().Split('\n'));
            //lines.RemoveAt(0);
            var index = GetNthIndex(url, '/', 6);
            url = url.Substring(0, index);

            for (int i = 0; i < lines.Count - 1; i++)
            {
                string line1 = lines[i];
                string line2 = lines[i + 1];

                var matchLine1 = Regex.Match(line1, "AVERAGE-BANDWIDTH=(?<band>[0-9]+),RESOLUTION=(?<res>[0-9x]+),");
                var matchLine2 = Regex.Match(line2, @"^[^#][\.\.\/]*(\/\d+\/.*m3u8)");

                if (matchLine1.Success && matchLine2.Success)
                {
                    var bandwidth = int.Parse(matchLine1.Groups["band"].Value);
                    var info = new StreamInfo()
                    {
                        Url = url + matchLine2.Groups[0].Value.Trim(new char[] { '.' }),
                        Bandwidth = bandwidth,
                        Resolution = matchLine1.Groups["res"].Value,
                        ApproxSize = (Duration * (bandwidth / 1024) / 1024 / 8),
                        StreamType = StreamType.VideoAndAudio,
                    };

                    Streams.Add(info);
                }
            }

            var list = Streams.OrderBy(s => s.Bandwidth).ToList();
            Streams.Clear();
            Streams.AddRange(list);
        }

        public int GetNthIndex(string s, char t, int n)
        {
            int count = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
