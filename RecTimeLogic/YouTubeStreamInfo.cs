using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecTimeLogic
{
    public class YouTubeStreamInfo : StreamInfo
    {
        private int _formatCode;

        public string Format { get; private set; }
        public string Quality { get; set; }
        public int FormatCode
        {
            get {  return _formatCode; }
            set
            {
                _formatCode = value;
                switch (value)
                {
                    case 34:
                    case 35:
                    case 5:
                    case 6:
                        Format = "Flash";
                        FileEnding = ".flv";
                        break;

                    case 18:
                    case 22:
                    case 37:
                    case 38:
                    case 82:
                    case 84:
                    case 135:
                    case 136:
                    case 137:
                        Format = "MP4";
                        FileEnding = ".mp4";
                        break;

                    case 13:
                    case 17:
                    case 36:
                        Format = "Mobile";
                        FileEnding = ".3gpp";
                        break;

                    case 43:
                    case 45:
                    case 46:
                        Format = "WebM";
                        FileEnding = ".webm";
                        break;
                    case 244:
                    case 247:
                    case 248:
                        Format = "WebM";
                        FileEnding = ".vp8";
                        break;
                    //139 = MP4 Audio, 44.1kHz 48kbps
                    //140 = MP4 Audio, 44.1kHz 128kbps
                    //141 = MP4 Audio, 44.1kHz 256kbps
                    case 139:
                    case 140:
                    case 141:
                        Format = "MP4 Audio";
                        FileEnding = ".mpa";
                        StreamType = StreamType.AudioOnly;
                        break;
                    //170 = WEBM Audio AAC, 44.1kHz 128kbps
                    //171 = WEBM Audio AAC, 44.1kHz 256kbps
                    case 170:
                    case 171:
                        Format = "WebM Audio";
                        FileEnding = ".aac";
                        StreamType = StreamType.AudioOnly;
                        break;
                    default:
                        Format = "Unknown (" + value + ")";
                        break;
                }
                
            }
        }

        public YouTubeStreamInfo(string url, string quality, int formatCode)
        {
            Quality = quality;
            FormatCode = formatCode;

            if (StreamType == StreamType.AudioOnly)
                AudioUrl = url;
            else
                Url = url;



            Resolution = Quality + " " + Format;
        }
    }
}
