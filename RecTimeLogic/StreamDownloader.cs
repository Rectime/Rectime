using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RecTimeLogic
{
    public class StreamDownloader : IStreamDownloader
    {
        public string Download(string url)
        {
            if (string.IsNullOrEmpty(url))
                return null;

            string data = string.Empty;

            WebClient wc = new WebClient();
            using (MemoryStream stream = new MemoryStream(wc.DownloadData(url)))
            {
                var sr = new StreamReader(stream);
                data = sr.ReadToEnd();
            }
            return data;
        }

        public Image DownloadImage(string url)
        {
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(url);
            MemoryStream ms = new MemoryStream(imageBytes);
            return Image.FromStream(ms);
        }
    }
}
