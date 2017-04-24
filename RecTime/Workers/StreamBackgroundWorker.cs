using System.ComponentModel;
using System.Diagnostics;
using RecTimeLogic;
using System.Threading;
using System;
using System.Text.RegularExpressions;

namespace RecTime
{
    class StreamBackgroundWorker : BackgroundWorker
    {
        private readonly StreamInfo _streamInfo;
        private readonly string _outputDirectory;
        private readonly string _outputFilename;
        string argFormat = @" -i ""{0}"" -acodec copy -vcodec copy -absf aac_adtstoasc ""{1}""";

        public string Duration { get; private set; }
        public int Percentage { get; private set; }
        public bool HasRun { get; private set; }

        public StreamBackgroundWorker(StreamInfo streamInfo, string outputDirectory, string outputFilename)
        {
            string invalidChars = Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            this._outputFilename = Regex.Replace(outputFilename, invalidRegStr, "_");
            this._outputDirectory = outputDirectory;
            this._streamInfo = streamInfo;
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            this.DoWork += DownloadStream;
        }

        private void DownloadStream(object sender, DoWorkEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "ffmpeg.exe";
            process.StartInfo.Arguments = string.Format(argFormat, _streamInfo.Url, _outputDirectory + @"\" + _outputFilename);
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
