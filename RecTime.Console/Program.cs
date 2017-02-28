using System;
using System.Diagnostics;
using MoreLinq;
using RecTimeLogic;
using System.Threading;

namespace RecTime.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string output = @"C:\users\Kefir\Desktop\test.mp4";
            string argFormat = @" -i ""{0}"" -acodec copy -vcodec copy -absf aac_adtstoasc ""{1}""";

            System.Console.Write("Url: ");
            string url = System.Console.ReadLine();
            StreamManager manager = new StreamManager(url, new StreamDownloader());

            System.Console.WriteLine("Downloading Data & stream info...");
            manager.DownloadAndParseData();
                
            var stream = manager.Streams.MaxBy(s => s.Bandwidth);
            System.Console.WriteLine("Selected stream: " + stream.Bandwidth + "kbit/s " + stream.Resolution);

            var streamUrl = stream.Url;

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = "ffmpeg.exe";
            p.StartInfo.Arguments = string.Format(argFormat, streamUrl, output);
            p.OutputDataReceived += P_OutputDataReceived;
            p.ErrorDataReceived += P_ErrorDataReceived;

            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.WaitForExit();
            System.Console.ReadKey();
        }

        private static void P_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            System.Console.WriteLine("error..." + e.Data);
        }

        private static void P_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            System.Console.WriteLine("output..." + e.Data);
        }
    }
}
