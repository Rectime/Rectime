using System.Drawing;

namespace RecTimeLogic
{
    public interface IStreamDownloader
    {
        string Download(string url);
        Image DownloadImage(string url);
    }
}