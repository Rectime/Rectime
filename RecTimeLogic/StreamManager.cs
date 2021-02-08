using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RecTimeLogic
{
    public class StreamManager
    {
        public List<StreamInfo> Streams { get; private set; }
        public string DataUrl { get; protected set; }
        public string ProgramTitle { get; private set; }
        public string Title { get; protected set; }
        public int Duration { get; protected set; }
        public string PosterUrl { get; protected set; }
        public Image PosterImage { get; protected  set; }
        public SourceType Type { get; protected set; }
        public string LongTitle => !string.IsNullOrEmpty(Title) ? Title : ProgramTitle + "-" + Title;
        public string BaseUrl { get; private set; }

        public string GetValidFileName(StreamInfo streamInfo)
        {
            string fileName = LongTitle + streamInfo.FileEnding;
            fileName = HttpUtility.HtmlDecode(fileName);

            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        protected string masterUrl;
        protected readonly IStreamDownloader streamDownloader;

        public StreamManager(string uri, IStreamDownloader downloader)
        {
            this.streamDownloader = downloader;
            Streams = new List<StreamInfo>();

            this.BaseUrl = uri;

            if (!string.IsNullOrEmpty(uri))
            {
                var result = UrlHelper.ParseUrl(uri);
                this.Type = result.Item1;
                this.DataUrl = result.Item2;
            }
        } 

        public virtual void DownloadAndParseData()
        {
            if (string.IsNullOrEmpty(DataUrl))
                return;

            string streamUrl = string.Empty;

            var data = streamDownloader.Download(BaseUrl);

            var videoIdMatch = Regex.Match(data, @"<video data-video-id=""(.+?)""");
            string videoId = "";

            if (videoIdMatch.Success)
                videoId = videoIdMatch.Groups[1].Value;
            else
            {
                var tryAgain = Regex.Match(data, @"data-video-id=""(.+?)""");
                if (tryAgain.Success)
                    videoId = tryAgain.Groups[1].Value;
            }

            //new Svtplay video Id
            if(string.IsNullOrEmpty(videoId))
            {
                var tryAgain = Regex.Match(data, @"__svtplay_apollo'\] = ({.*});");
                if (tryAgain.Success)
                {
                    dynamic json = JObject.Parse(tryAgain.Groups[1].Value);
                    JObject root = json.ROOT_QUERY;
                    var name = root.Properties().FirstOrDefault(p => p.Name.StartsWith("listablesBy"));
                    var tmpId = root[name.Name][0]["id"].ToString();
                    videoId = json[tmpId].videoSvtId;
                }
                else
                {
                    var newFix = Regex.Match(data, @"<script id=""__NEXT_DATA__"" type=""application/json"">({.+})</script>");
                    if(newFix.Success)
                    {
                        var json = JObject.Parse(newFix.Groups[1].Value);
                        foreach (var subData in json["props"]["urqlState"])
                        {
                            var subJson = JObject.Parse(subData.Children()["data"].First().Value<string>());
                            JToken token = subJson.SelectToken("listablesByEscenicId[0].videoSvtId", errorWhenNoMatch: false);
                            if(token != null)
                            {
                                videoId = token.ToString();
                                break;
                            }
                            //try
                            //{
                                //videoId = subJson["listablesByEscenicId"][0]["videoSvtId"].ToString();
                            //    break;
                            //}
                            //catch { }
                        }

                    }
                }
            }


            var titleMatch = Regex.Match(data, @"<title( data-react-helmet=""true"")?>(.+?)<");
            if (titleMatch.Success)
            {
                Title = titleMatch.Groups[2].Value;
                if (Title.Contains("|"))
                    Title = Title.Substring(0, Title.IndexOf('|') - 1);
            }
            else
                Title = "unknown";

            var imageMatch = Regex.Match(data, @"""thumbnailUrl"" content=""(.+?)""");
            if (imageMatch.Success)
            {
                PosterUrl = imageMatch.Groups[1].Value;
                PosterImage = streamDownloader.DownloadImage(PosterUrl);
            }
            else
            {
                var posterMatch = Regex.Match(data, @"poster=""(.+?)""");
                if (posterMatch.Success)
                {
                    PosterUrl = posterMatch.Groups[1].Value;
                    PosterImage = streamDownloader.DownloadImage(PosterUrl);
                }
            }

            if(Type == SourceType.ÖppetArkiv || Type == SourceType.Svt)
                data = streamDownloader.Download("https://api.svt.se/videoplayer-api/video/" + videoId);
            else
                data = streamDownloader.Download("https://api.svt.se/video/" + videoId);

            var programTitleMatch = Regex.Match(data, @"""programTitle"":""(.+?)""");
            if (programTitleMatch.Success)
                ProgramTitle = programTitleMatch.Groups[1].Value;

            var durationMatch = Regex.Match(data, @"""contentDuration"":(\d+)");
            if (durationMatch.Success)
                Duration = int.Parse(durationMatch.Groups[1].Value);

            //obsolete check ?
            var match = Regex.Match(data, "https:[^\\s:\"\\?]+master\\.m3u8");
            if (match.Success)
            {
                masterUrl = match.Captures[0].Value;
                Streams.Clear();
                ParseStreams(streamDownloader.Download(masterUrl));
            }
            else
            {
                Streams.Clear();
                dynamic json = JObject.Parse(data);
                foreach(var item in json.videoReferences)
                {
                    if(item.format.ToString().StartsWith("hls"))
                    {
                        masterUrl = item.url.ToString();
                        ParseStreams(streamDownloader.Download(item.url.ToString()));
                    }
                }
                /*var matches = Regex.Matches(data, @"url"":\s+""(https:[^\s:\""\?]+full\.m3u8)");
                foreach(Match m in matches)
                {
                    masterUrl = m.Captures[1].Value;
                    ParseStreams(streamDownloader.Download(masterUrl));
                }*/
            }
            Streams = Streams.GroupBy(s => s.ToString()).Select(x => x.First()).ToList();
            Streams = Streams.OrderBy(s => s.ApproxSize).ThenBy(s => s.Extra).ToList();
        }

        protected void ParseStreams(string data)
        {
            List<StreamInfo> streams = new List<StreamInfo>();
            if (string.IsNullOrEmpty(data))
                return;

            //Subtitles?
            string subtitleUrl = null;

            var subtitlematch = Regex.Match(data, "#EXT-X-MEDIA:TYPE=SUBTITLES(.+)URI=\"(?<subtitle>[^\"]+)\"");
            if (subtitlematch.Success)
            {
                subtitleUrl = subtitlematch.Groups["subtitle"].Value;
            }

            //Separate audio?
            var audio = new Dictionary<string, Tuple<string, string>>();

            //SvtPlay / Öppet Arkiv
            var audiomatches = Regex.Matches(data, "#EXT-X-MEDIA:TYPE=AUDIO(.+)URI=\"(?<audio>[^\"]+)\"(.+)GROUP-ID=\"(?<id>[^\"]+)\"(.*)CHANNELS=\"(?<channels>[^\"]+)\"");
            foreach(Match audiomatch in audiomatches)
            {
                if(audiomatch.Success)
                {
                    if(!audio.ContainsKey(audiomatch.Groups["id"].Value))
                        audio.Add(audiomatch.Groups["id"].Value, new Tuple<string, string>(audiomatch.Groups["audio"].Value, audiomatch.Groups["channels"].Value));
                }
            }

            //Tv4Play
            if (audio.Count == 0)
            {
                var audiomatch2 = Regex.Match(data, "#EXT-X-MEDIA:TYPE=AUDIO(.+)URI=\"(?<audio>[^\"]+)\"");
                if (audiomatch2.Success)
                {
                    audio.Add("default", new Tuple<string, string>(audiomatch2.Groups["audio"].Value, "2"));
                }
            }

            var lines = new List<string>(data.Trim().Split('\n'));
            //lines.RemoveAt(0);

            for (int i = 0; i < lines.Count - 1; i++)
            {
                string line1 = lines[i];
                string line2 = lines[i + 1];

                var matchLine1 = Regex.Match(line1, "BANDWIDTH=(?<band>[0-9]+),RESOLUTION=(?<res>[0-9x]+),CODECS=\"(?<codec>.+)\"");
                var matchLine2 = Regex.Match(line2, "^[^#](.+)m3u8");

                // Try again, new pattern
                if (!matchLine1.Success)
                {
                    matchLine1 = Regex.Match(line1, "BANDWIDTH=(?<band>[0-9]+),CODECS=\"(?<codec>.+)\",RESOLUTION=(?<res>[0-9x]+)");
                }

                if (matchLine1.Success && matchLine2.Success)
                {
                    var bandwidth = int.Parse(matchLine1.Groups["band"].Value);
                    var info = new StreamInfo()
                    {
                        Url = (matchLine2.Groups[0].Value.ToLower().StartsWith("http")) 
                            ? matchLine2.Groups[0].Value : 
                            UrlHelper.GetBaseMasterUrl(masterUrl) + matchLine2.Groups[0].Value,
                        Bandwidth = bandwidth,
                        Resolution = matchLine1.Groups["res"].Value,
                        Codec = matchLine1.Groups["codec"].Value,
                        ApproxSize = (Duration * (bandwidth / 1024) / 1024 / 8),
                        StreamType = StreamType.VideoAndAudio
                        //StreamType = (string.IsNullOrEmpty(audioUrl)) ? StreamType.VideoAndAudio : StreamType.VideoSeparateAudio,
                        //AudioUrl = (audioUrl != null && audioUrl.ToLower().StartsWith("http")) ? audioUrl : UrlHelper.GetBaseMasterUrl(masterUrl) + audioUrl,
                    };

                    if (subtitleUrl != null)
                        info.SubtitleUrl = (subtitleUrl.ToLower().StartsWith("http")) ? subtitleUrl : UrlHelper.GetBaseMasterUrl(masterUrl) + subtitleUrl;

                    streams.Add(info);
                }
            }

            // remove duplicates..
            // add audio
            var distinct = streams.GroupBy(s => s.Url).Select(x => x.First()).ToList();

            //Separate audio?
            if (audio.Count > 0)
            {
                streams.Clear();
                foreach (var audioInfo in audio)
                {
                    foreach(var stream in distinct)
                    {
                        var copy = stream.ShallowCopy();
                        copy.StreamType = StreamType.VideoSeparateAudio;
                        copy.AudioUrl = (audioInfo.Value.Item1.ToLower().StartsWith("http")) ? audioInfo.Value.Item1 : UrlHelper.GetBaseMasterUrl(masterUrl) + audioInfo.Value.Item1;
                        copy.Extra = audioInfo.Value.Item2 + "CH";
                        streams.Add(copy);
                    }
                }
            }

            Streams.AddRange(streams);

        }
    }
}
