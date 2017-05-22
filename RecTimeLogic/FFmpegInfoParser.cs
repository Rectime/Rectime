using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecTimeLogic
{
    public class FFmpegInfoParser
    {
        public static FFmpegInfo Parse(string line)
        {
            if (string.IsNullOrEmpty(line))
                return null;

            var duration = Regex.Match(line, "Duration: ([0-9:.]+)", RegexOptions.Singleline);

            if (duration.Success)
            {
                return new FFmpegInfo()
                {
                    Duration  = duration.Groups[1].Value
                };
            }

            var match = Regex.Match(line, "frame=\\s*([0-9]+)\\s*fps=\\s*([0-9.]+).+size=\\s*([0-9]+)kB\\s*time=([0-9:.]+)\\s*bitrate=\\s*([0-9.]+)kbit.+speed=\\s*([0-9.]+)x");

            if (match.Success)
            {
                return new FFmpegInfo()
                {
                    Frame = int.Parse(match.Groups[1].Value),
                    Fps = double.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture),
                    Size = int.Parse(match.Groups[3].Value),
                    Time = match.Groups[4].Value,
                    Bitrate = double.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture),
                    Speed = double.Parse(match.Groups[6].Value, CultureInfo.InvariantCulture)
                };
            }
            else
                return null;
        }
    }

    public class FFmpegInfo
    {
        public int Frame { get; set; }
        public double Fps { get; set; }
        public int Size { get; set; }
        public string Time { get; set; }
        public double Bitrate { get; set; }
        public double Speed { get; set; }
        public string Duration { get; set; }

        public override string ToString()
        {
            return string.Format("Time: {0} Size: {1}kB Speed: {2}x", this.Time, this.Size, this.Speed);
        }
    }
}
