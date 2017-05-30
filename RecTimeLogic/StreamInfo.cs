using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecTimeLogic
{
    public class StreamInfo
    {
        public string Url { get; set; }
        public string AudioUrl { get; set; }
        public string Resolution { get; set; }
        public int Bandwidth { get; set; }
        public string Codec { get; set; }
        public int ApproxSize { get; set; }
        public string Extra { get; set; }
        public string FileEnding { get; set; }
        public StreamType StreamType { get; set; }

        public StreamInfo()
        {
            StreamType = StreamType.VideoAndAudio;
            FileEnding = ".mp4";
        }

        public override string ToString()
        {
            return Resolution + " " + Bandwidth/1000 + "kbit/s (" + ApproxSize + "MB) " + Extra;
        }
    }

    public enum StreamType
    {
        VideoAndAudio,
        AudioOnly,
        VideoSeparateAudio
    }
}
