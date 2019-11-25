using System.ComponentModel;
using System.Data;
using RecTimeLogic;

namespace RecTime
{
    class InfoBackgroundWorker : BackgroundWorker
    {
        protected string Url = string.Empty;

        public StreamManager Manager { get; private set; }

        public InfoBackgroundWorker(string url)
        {
            Url = url;
            var type = UrlHelper.ParseUrl(Url);

            switch (type.Item1)
            {
                case SourceType.UrPlay:
                case SourceType.UrSkola:
                case SourceType.Svt1Live:
                case SourceType.Svt24Live:
                case SourceType.Svt2Live:
                case SourceType.SvtBarnLive:
                case SourceType.SvtKunskapLive:
                    Manager = new UrStreamManager(Url, new StreamDownloader());
                    break;
                case SourceType.YouTube:
                    Manager = new YouTubeStreamManager(Url, new StreamDownloader());
                    break;
                case SourceType.Tv4Play:
                    Manager = new Tv4StreamManager(url, new StreamDownloader());
                    break;
                case SourceType.Vimeo:
                    Manager = new VimeoStreamManager(url, new StreamDownloader());
                    break;
                default:
                    Manager = new StreamManager(Url, new StreamDownloader());
                    break;
            }

            this.DoWork += DownloadInfo;
        }

        protected virtual void DownloadInfo(object sender, DoWorkEventArgs e)
        {
            Manager.DownloadAndParseData();
        }
    }
}
