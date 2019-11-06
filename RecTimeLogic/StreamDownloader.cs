using System.Drawing;
using System.IO;
using System.Net;

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
            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

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
