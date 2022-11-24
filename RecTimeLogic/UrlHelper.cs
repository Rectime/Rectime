using System;
using System.Text.RegularExpressions;
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
            else if (url.ToLower().Contains("svt.se"))
            {
                type = SourceType.Svt;
                dataUrl = url;
            }
            else if (url.ToLower().Contains("tv4play.se") || url.ToLower().Contains("tv4.se"))
                type = SourceType.Tv4Play;
            else if (url.ToLower().Contains("urplay.se") || url.ToLower().Contains("url.se"))
                type = SourceType.UrPlay;
            else if (url.ToLower().Contains("urskola.se"))
                type = SourceType.UrSkola;
            else if (url.ToLower().Contains("youtube"))
            {
                type = SourceType.YouTube;
                dataUrl = url;
            }
            else if(url.ToLower().Contains("vimeo.com") )
            {
                type = SourceType.Vimeo;
                dataUrl = url;
            }

            return new Tuple<SourceType, string>(type, dataUrl);
        }

        public static string GetBaseMasterUrl(string url)
        {
            return url.Substring(0, url.LastIndexOf("/") + 1);
        }

        private static string FindDataUrl(string url)
        {
            string ret = string.Empty;

            var match = Regex.Match(url, "video/([0-9a-zA-Z]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                ret = url.Substring(0, url.ToLower().IndexOf("/video/") + 7);
                ret += match.Groups[1].Value + "?output=json";
                return ret;
            }
            // url /klipp/ .. ?

            match = Regex.Match(url, "klipp/([0-9a-zA-Z]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                ret = url.Substring(0, url.ToLower().IndexOf("/klipp/") + 7);
                ret += match.Groups[1].Value + "?output=json";
            }

            return ret;
        }
    }
}
