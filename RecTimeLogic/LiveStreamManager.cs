 using System;
 using System.CodeDom;
 using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecTimeLogic
{
    public class LiveStreamManager : StreamManager
    {
        private readonly Dictionary<string, Tuple<SourceType, string>> _channels =
            new Dictionary<string, Tuple<SourceType, string>>()
            {
                { "svt1", new Tuple<SourceType, string>(SourceType.Svt1Live, "http://www.svtplay.se/kanaler/svt1") }
            };


        public LiveStreamManager(string channel, IStreamDownloader downloader) : base(null, downloader)
        {
            if (_channels.ContainsKey(channel))
            {
                var t = _channels[channel];
                this.Type = t.Item1;
                this.DataUrl = t.Item2;
            }
        }
    }
}
