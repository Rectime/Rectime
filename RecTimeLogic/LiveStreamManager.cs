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
                { SourceType.Svt1Live, "https://www.svtplay.se/kanaler/svt1?output=json" },
                { SourceType.Svt2Live, "https://www.svtplay.se/kanaler/svt2?output=json" },
                { SourceType.Svt24Live, "https://www.svtplay.se/kanaler/svt24?output=json" },
                { SourceType.SvtBarnLive, "https://www.svtplay.se/kanaler/barnkanalen?output=json" },
                { SourceType.SvtKunskapLive, "https://www.svtplay.se/kanaler/kunskapskanalen?output=json" }
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

            var lines = new List<string>(data.Trim().Split('\n'));
            lines.RemoveAt(0);
            lines.RemoveAt(0);

            for (int i = 0; i < lines.Count / 2; i++)
            {
                string line1 = lines[i * 2];
                string line2 = lines[i * 2 + 1];

                var matchLine1 = Regex.Match(line1, "RESOLUTION=([0-9x]+),BANDWIDTH=([0-9]+)");

                if (matchLine1.Success)
                {
                    var bandwidth = int.Parse(matchLine1.Groups[2].Value);
                    var info = new StreamInfo()
                    {
                        Url = UrlHelper.GetBaseMasterUrl(masterUrl) + line2,
                        Bandwidth = bandwidth,
                        Resolution = matchLine1.Groups[1].Value,
                        ApproxSize = (Duration * (bandwidth / 1024) / 1024 / 8)
                    };
                    Streams.Add(info);
                }
            }
        }
    }
}
