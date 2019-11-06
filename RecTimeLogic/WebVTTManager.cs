using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace RecTimeLogic
{
    public class WebVTTManager
    {
        private string _playList;
        private string _baseUrl;
        private const int mpeg2time = 90000;
        private readonly string[] _delimiters = new string[] { "-->", "- >", "->" };

        private string _output;
        private int _sequence;

        protected readonly IStreamDownloader streamDownloader;

        public int MpegTs { get; set; }
        public string Output => _output;
        public bool IsVTT { get; set; }

        public WebVTTManager(string url, IStreamDownloader downloader)
        {
            _playList = url;
            _baseUrl = url.Substring(0, url.LastIndexOf('/') + 1);
            streamDownloader = downloader;
        }

        public void DownloadAndProcess()
        {
            _output = string.Empty;
            _sequence = 0;
            var playlist = streamDownloader.Download(_playList);
            MpegTs = 0;

            if(_playList.StartsWith("https://svt"))
            {
                var vttFile = _playList.Replace(".m3u8", ".vtt");
                _output = streamDownloader.Download(vttFile);
                IsVTT = true;
                return;
            }

            var mpegts = Regex.Match(playlist, "#USP-X-TIMESTAMP-MAP:MPEGTS=(?<rate>[0-9]+)");
            if (mpegts.Success)
            {
                MpegTs = int.Parse(mpegts.Groups["rate"].Value);
            }

            string[] lines = playlist.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                if (!line.EndsWith(".webvtt"))
                    continue;

                var segment = streamDownloader.Download(_baseUrl + line);
                ParseSegment(segment);
            }

            Debug.WriteLine(_output);
        }

        private void ParseSegment(string segment)
        {
            int timeOffset = 0;

            var mpegts = Regex.Match(segment, "X-TIMESTAMP-MAP=MPEGTS:(?<offset>[0-9]+)");
            if (mpegts.Success)
            {
                timeOffset = int.Parse(mpegts.Groups["offset"].Value) - MpegTs;
            }

            double secondsOffset = timeOffset / (double)mpeg2time;
            Debug.WriteLine("Seconds offset: " + secondsOffset);

            var lines = segment.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None).Skip(3);
            foreach(var line in lines)
            {
                var parts = line.Split(_delimiters, StringSplitOptions.None);
                if (parts.Length != 2)
                {
                    // this is not a timecode line
                    if(!string.IsNullOrEmpty(line.Trim()))
                        _output += line + Environment.NewLine;
                }
                else
                {
                    if (_sequence > 0)
                        _output += Environment.NewLine;

                    _sequence++;
                    _output += _sequence + Environment.NewLine;
                    var startTc = ParseVttTimecode(parts[0]).Add(TimeSpan.FromSeconds(secondsOffset));
                    var endTc = ParseVttTimecode(parts[1]).Add(TimeSpan.FromSeconds(secondsOffset));
                    _output += $"{startTc:hh\\:mm\\:ss\\.fff} --> {endTc:hh\\:mm\\:ss\\.fff}" + Environment.NewLine;
                }
            }
        }

        private TimeSpan ParseVttTimecode(string s)
        {
            string timeString = string.Empty;
            var match = Regex.Match(s, "[0-9]+:[0-9]+:[0-9]+[,\\.][0-9]+");
            if (match.Success)
            {
                timeString = match.Value;
            }
            else
            {
                match = Regex.Match(s, "[0-9]+:[0-9]+[,\\.][0-9]+");
                if (match.Success)
                {
                    timeString = "00:" + match.Value;
                }
            }

            if (!string.IsNullOrEmpty(timeString))
            {
                timeString = timeString.Replace(',', '.');
                TimeSpan result;
                if (TimeSpan.TryParse(timeString, out result))
                {
                    var nbOfMs = (int)result.TotalMilliseconds;
                    return result;
                }
            }

            return TimeSpan.MinValue;
        }
    }
}
