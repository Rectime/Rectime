using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RecTimeLogic
{
    [XmlRoot("tv")]
    public class ChannelInfo
    {
        [XmlElement("programme")]
        public List<ProgramInfo> Programs { get; set; }

        public ChannelInfo()
        {
            Programs = new List<ProgramInfo>();
        }
    }

    public class ProgramInfo
    {
        [XmlElement("title")]
        public string Title { get; set; }
    }
}
