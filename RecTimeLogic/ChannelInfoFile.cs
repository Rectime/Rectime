using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecTimeLogic
{
    public class ChannelInfoFile
    {
        public SourceType Source { get; set; }
        public string FileName { get; set; }

        public static ChannelInfoFile FromSourceTypeAndDate(SourceType type, DateTime date)
        {
            string prefix = "";

            switch (type)
            {
                    case SourceType.Svt1Live: prefix="svt1.svt.se_"; break;
                    case SourceType.Svt2Live: prefix="svt2.svt.se_"; break;
                    case SourceType.Svt24Live: prefix="svt24.svt.se_"; break;
                    case SourceType.SvtBarnLive: prefix="svtb.svt.se_"; break;
                    case SourceType.SvtKunskapLive: prefix="kunskapskanalen.svt.se_"; break;
            }

            return new ChannelInfoFile()
            {
                Source = type,
                FileName = prefix + date.ToShortDateString() + ".xml.gz"
            };
        }
    }
}
