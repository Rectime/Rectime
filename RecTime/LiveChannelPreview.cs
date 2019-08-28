using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using RecTimeLogic;

namespace RecTime
{
    class LiveChannelPreview
    {
        public readonly SourceType Channel;
        private readonly Process _ffplay = new Process();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        public Control Parent { get; }
        public bool IsActivated { get; set; }

        public LiveChannelPreview(Control parent, SourceType channel)
        {
            Parent = parent;
            Channel = channel;
        }

        public void Activate()
        {
            if (IsActivated)
                return;

            LiveStreamManager mgr = new LiveStreamManager(Channel, new StreamDownloader());
            mgr.DownloadAndParseData();
            var lowestStream = mgr.Streams.OrderBy(s => s.Bandwidth).FirstOrDefault();

            if (lowestStream == null)
                return;

            _ffplay.StartInfo.FileName = "ffplay.exe";
            _ffplay.StartInfo.Arguments = $"{lowestStream.Url} -noborder -rtbufsize 15M -x 160 -y 90";
            _ffplay.StartInfo.CreateNoWindow = true;
            _ffplay.StartInfo.RedirectStandardOutput = true;
            _ffplay.StartInfo.UseShellExecute = false;
            _ffplay.EnableRaisingEvents = true;
            _ffplay.OutputDataReceived += (o, e) => Debug.WriteLine(e.Data ?? "NULL", "ffplay");
            _ffplay.ErrorDataReceived += (o, e) => Debug.WriteLine(e.Data ?? "NULL", "ffplay");
            _ffplay.Exited += (o, e) => Debug.WriteLine("Exited", "ffplay");
            _ffplay.Start();


            while ((int)_ffplay.MainWindowHandle == 0)
            {
                Thread.Sleep(0);
            }

            // child, new parent
            // make 'this' the parent of ffmpeg (presuming you are in scope of a Form or Control)
            SetParent(_ffplay.MainWindowHandle, Parent.Handle);

            // window, x, y, width, height, repaint
            // move the ffplayer window to the top-left corner and set the size to 320x280
            MoveWindow(_ffplay.MainWindowHandle, 0, 0, 160, 90, true);
            IsActivated = true;
        }

        public void Kill()
        {
            if (IsActivated)
            {
                try { _ffplay.Kill(); }
                catch
                {
                    // ignored
                }

                IsActivated = false;
            }
        }
    }
}
