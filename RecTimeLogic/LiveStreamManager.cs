 using System;
 using System.CodeDom;
 using System.Collections.Generic;
using System.Linq;
using System.Text;
 using System.Text.RegularExpressions;
 using System.Threading.Tasks;

namespace RecTimeLogic
{
    public class LiveStreamManager : StreamManager
    {
        private readonly Dictionary<SourceType, string> _channels =
            new Dictionary<SourceType, string>()
            {
                { SourceType.Svt1Live, "https://api.svt.se/video/ch-svt1" },
                { SourceType.Svt2Live, "https://api.svt.se/video/ch-svt2" },
                { SourceType.Svt24Live, "https://api.svt.se/video/ch-svt24" },
                { SourceType.SvtBarnLive, "https://api.svt.se/video/ch-barnkanalen" },
                { SourceType.SvtKunskapLive, "https://api.svt.se/video/ch-kunskapskanalen" }
            };

        
        public LiveStreamManager(SourceType type, IStreamDownloader downloader) : base(null, downloader)
        {
            if (_channels.ContainsKey(type))
            {
                this.Type = type;
                this.DataUrl = _channels[type];
            }
        }

        public override void DownloadAndParseData()
        {
            var data = streamDownloader.Download(DataUrl);

            var match = Regex.Match(data, "https:[^\\s:\"\\?]+\\.m3u8");
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
           
            var pattern =
                @"#EXT-X-STREAM-INF:BANDWIDTH=([0-9]+),CODECS=""(.+?)"",RESOLUTION=([0-9x]+)(,AUDIO=""audio"")?[\r\n]+(.+?)[\r\n]";
            var matches = Regex.Matches(data, pattern);

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var bandwidth = int.Parse(match.Groups[1].Value);
                    var info = new StreamInfo()
                    {
                        Url = UrlHelper.GetBaseMasterUrl(masterUrl) + match.Groups[match.Groups.Count -1 ].Value,
                        Bandwidth = bandwidth,
                        Resolution = match.Groups[3].Value,
                        ApproxSize = (Duration * (bandwidth / 1024) / 1024 / 8)
                    };
                    Streams.Add(info);
                }
           
            }
        }
    }
}
