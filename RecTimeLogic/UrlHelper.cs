using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace RecTimeLogic
{
    public class UrlHelper
    {
        public static Tuple<SourceType, string> ParseUrl(string url)
        {
            SourceType type = SourceType.Unknown;
            string dataUrl = string.Empty;

            if (url.ToLower().Contains("svtplay.se"))
            {
                type = SourceType.SvtPlay;
                dataUrl = FindDataUrl(url);
            }
            else if (url.ToLower().Contains("oppetarkiv.se"))
            {
                type = SourceType.ÖppetArkiv;
                dataUrl = FindDataUrl(url);
            }
            else if (url.ToLower().Contains("tv4play.se"))
                type = SourceType.Tv4Play;
            else if (url.ToLower().Contains("urplay.se"))
                type = SourceType.UrPlay;
            else if (url.ToLower().Contains("urskola.se"))
                type = SourceType.UrSkola;
            else if (url.ToLower().Contains("youtube"))
            {
                type = SourceType.YouTube;
                dataUrl = FindYoutubeDataUrl(url);
            }

            return new Tuple<SourceType, string>(type, dataUrl);
        }

        public static string GetBaseMasterUrl(string url)
        {
            return url.Substring(0, url.LastIndexOf("/") + 1);
        }

        private const string YouTubeInfoPageUrl = "http://www.youtube.com/get_video_info?&video_id={0}&el=detailpage&ps=default&eurl=&gl=US&hl=en";

        private static string FindYoutubeDataUrl(string url)
        {
            var ret = string.Empty;
            var id = HttpUtility.ParseQueryString(new Uri(url).Query)["v"];
            return string.IsNullOrEmpty(id) ? string.Empty : string.Format(YouTubeInfoPageUrl, id);
        }

        private static string FindDataUrl(string url)
        {
            string ret = string.Empty;

            var match = Regex.Match(url, "video/([0-9]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                ret = url.Substring(0, url.ToLower().IndexOf("/video/") + 7);
                ret += match.Groups[1].Value + "?output=json";
            }
            // url /klipp/ .. ?

            return ret;
        }
    }
}
