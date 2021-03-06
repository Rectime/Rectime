﻿using System.ComponentModel;
using System.Diagnostics;
using RecTimeLogic;
using System.Threading;
using System;
using System.Text.RegularExpressions;
using System.IO;

namespace RecTime
{
    class StreamBackgroundWorker : BackgroundWorker
    {
        protected StreamInfo StreamInfo;
        private readonly string _outputDirectory;
        string argFormat = @" -i ""{0}"" {2} -c copy ""{1}""";
        string argFormatSeparateAudio = @" -i ""{0}"" -i ""{3}"" {2} -c copy ""{1}""";
        string argFormatAudioOnly = @" -i ""{0}"" {2} ""{1}""";

        public string OutputFilename { get; private set; }
        public string Duration { get; protected set; }
        public int Percentage { get; private set; }
        public bool HasRun { get; protected set; }
        public TimeSpan? TimeLimit { get; set; }
        public virtual bool IsChannelRecorder => false;
        public bool ForceSrt { get; private set; }

        public StreamBackgroundWorker(StreamInfo streamInfo, string outputDirectory, string outputFilename, bool forceSrt)
        {
            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            ForceSrt = forceSrt;
            this.OutputFilename = Regex.Replace(outputFilename, invalidRegStr, "_");
            this._outputDirectory = outputDirectory;
            this.StreamInfo = streamInfo;
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            this.DoWork += DownloadStream;
        }

        protected virtual void DownloadStream(object sender, DoWorkEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "ffmpeg.exe";
            var timeLimit = TimeLimit == null ? "" : "-t "+TimeLimit.Value;

            var args = string.Empty;

            switch (StreamInfo.StreamType)
            {
                case StreamType.AudioOnly:
                    args = string.Format(argFormatAudioOnly, StreamInfo.AudioUrl, _outputDirectory + @"\" + OutputFilename, timeLimit);
                    break;
                case StreamType.VideoSeparateAudio:
                    args = string.Format(argFormatSeparateAudio, StreamInfo.Url, _outputDirectory + @"\" + OutputFilename, timeLimit, StreamInfo.AudioUrl);
                    break;
                default:
                    args = string.Format(argFormat, StreamInfo.Url, _outputDirectory + @"\" + OutputFilename, timeLimit);
                    break;
            }

            if (!string.IsNullOrEmpty(StreamInfo.SubtitleUrl))
            {
                ReportProgress(0, "Downloading subtitles");
                var subtitleManager = new WebVTTManager(StreamInfo.SubtitleUrl, new StreamDownloader());
                subtitleManager.DownloadAndProcess(ForceSrt);
                var fileName = _outputDirectory + @"\";
                fileName += (subtitleManager.IsVTT) ? OutputFilename.Replace(".mp4", ".vtt") : OutputFilename.Replace(".mp4", ".srt");
                File.WriteAllText(fileName, subtitleManager.Output);
            }
            process.StartInfo.Arguments = args;
            process.ErrorDataReceived += ProcessErrorDataReceived;
            process.OutputDataReceived += ProcessOutputDataReceived;

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            
            while (!process.HasExited)
            {
                Thread.Sleep(0);
                if (CancellationPending)
                    process.Kill();
            }

            process.Close();
            HasRun = true;
        }

        private void ProcessOutput(string line)
        {
            try
            {
                var data = FFmpegInfoParser.Parse(line);

                if (data != null)
                {
                    if (!string.IsNullOrEmpty(data.Duration))
                        Duration = data.Duration;
                    ReportProgress(0, data);
                }
                else
                    Debug.WriteLine("Can't parse line: " + line);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Exception parsing lines: " + line);
                Debug.WriteLine(ex.Message);
            }
        }

        private void ProcessOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            ProcessOutput(e.Data);
        }

        private void ProcessErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            ProcessOutput(e.Data);
        }
    }
}
