using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            return new Tuple<SourceType, string>(type, dataUrl);
        }

        public static string GetBaseMasterUrl(string url)
        {
            return url.Substring(0, url.LastIndexOf("/") + 1);
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
