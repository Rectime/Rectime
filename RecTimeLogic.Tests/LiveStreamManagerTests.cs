using System;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecTimeLogic.Tests
{
    [TestClass]
    public class LiveStreamManagerTests
    {
        private LiveStreamManager _underTest;
        private IStreamDownloader _downloader;

        private string _channelInfoExample =
            @"{""svtId"":""ch-svt1"",""programVersionId"":""ch-svt1"",""contentDuration"":0,""blockedForChildren"":false,""live"":false,""simulcast"":true,""programTitle"":""Kanaler"",""episodeTitle"":""svt1"",""rights"":{""drmCopyProtection"":false,""blockedForMobile"":false,""geoBlockedSweden"":true},""videoReferences"":[{""url"":""https://svt1-b.akamaized.net/se/svt1/master.m3u8?defaultSubLang=1"",""format"":""hlswebvtt""},{""url"":""https://svt1-b.akamaized.net/se/svt1/master.m3u8?defaultSubLang=1"",""format"":""hls""},{""url"":""https://svt1-b.akamaized.net/se/svt1/manifest.mpd?defaultSubLang=1"",""format"":""dashhbbtv""}],""mmsStatistics"":{""mms_tid"":""svt1"",""ns_st_pl"":""simulcast"",""ns_st_pr"":""kanaler"",""ns_st_ep"":""svt1"",""ns_st_cl"":0,""ns_st_li"":1,""svt_contenttype"":""simulcast"",""svt_content_type"":""simulcast""}}";

        private string _streamInfoExample = @"#EXTM3U
#EXT-X-VERSION:4
#EXT-X-MEDIA:TYPE=AUDIO,GROUP-ID=""audio"",LANGUAGE=""sv"",NAME=""Originalljud"",DEFAULT=YES,AUTOSELECT=YES
#EXT-X-MEDIA:TYPE=AUDIO,URI=""36603052-54ff-4835-a041-a1c25feee9a1/hls-a0/a0.m3u8"",GROUP-ID=""audio"",LANGUAGE=""sv-tal"",NAME=""Uppläst text"",DEFAULT=NO,AUTOSELECT=YES
#EXT-X-STREAM-INF:BANDWIDTH=732000,CODECS=""avc1.42c016,mp4a.40.2"",RESOLUTION=512x288,AUDIO=""audio""
36603052-54ff-4835-a041-a1c25feee9a1/hls-v3/v3.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=240000,CODECS=""avc1.42c015,mp4a.40.2"",RESOLUTION=512x288,AUDIO=""audio""
36603052-54ff-4835-a041-a1c25feee9a1/hls-v0/v0.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=444000,CODECS=""avc1.42c016,mp4a.40.2"",RESOLUTION=512x288,AUDIO=""audio""
36603052-54ff-4835-a041-a1c25feee9a1/hls-v1/v1.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=552000,CODECS=""avc1.42c016,mp4a.40.2"",RESOLUTION=512x288,AUDIO=""audio""
36603052-54ff-4835-a041-a1c25feee9a1/hls-v2/v2.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=1084000,CODECS=""avc1.4d401e,mp4a.40.2"",RESOLUTION=768x432,AUDIO=""audio""
36603052-54ff-4835-a041-a1c25feee9a1/hls-v4/v4.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=1776000,CODECS=""avc1.4d401f,mp4a.40.2"",RESOLUTION=1280x720,AUDIO=""audio""
36603052-54ff-4835-a041-a1c25feee9a1/hls-v5/v5.m3u8
#EXT-X-STREAM-INF:BANDWIDTH=2892000,CODECS=""avc1.4d401f,mp4a.40.2"",RESOLUTION=1280x720,AUDIO=""audio""
36603052-54ff-4835-a041-a1c25feee9a1/hls-v6/v6.m3u8
#EXT-X-I-FRAME-STREAM-INF:URI=""36603052-54ff-4835-a041-a1c25feee9a1/hls-v0/v0-i.m3u8"",BANDWIDTH=14400,CODECS=""avc1.42c015"",RESOLUTION=512x288
#EXT-X-I-FRAME-STREAM-INF:URI=""36603052-54ff-4835-a041-a1c25feee9a1/hls-v1/v1-i.m3u8"",BANDWIDTH=34800,CODECS=""avc1.42c016"",RESOLUTION=512x288
#EXT-X-I-FRAME-STREAM-INF:URI=""36603052-54ff-4835-a041-a1c25feee9a1/hls-v2/v2-i.m3u8"",BANDWIDTH=45600,CODECS=""avc1.42c016"",RESOLUTION=512x288
#EXT-X-I-FRAME-STREAM-INF:URI=""36603052-54ff-4835-a041-a1c25feee9a1/hls-v3/v3-i.m3u8"",BANDWIDTH=63600,CODECS=""avc1.42c016"",RESOLUTION=512x288
#EXT-X-I-FRAME-STREAM-INF:URI=""36603052-54ff-4835-a041-a1c25feee9a1/hls-v4/v4-i.m3u8"",BANDWIDTH=98800,CODECS=""avc1.4d401e"",RESOLUTION=768x432
#EXT-X-I-FRAME-STREAM-INF:URI=""36603052-54ff-4835-a041-a1c25feee9a1/hls-v5/v5-i.m3u8"",BANDWIDTH=168000,CODECS=""avc1.4d401f"",RESOLUTION=1280x720
#EXT-X-I-FRAME-STREAM-INF:URI=""36603052-54ff-4835-a041-a1c25feee9a1/hls-v6/v6-i.m3u8"",BANDWIDTH=279600,CODECS=""avc1.4d401f"",RESOLUTION=1280x720";

        [TestInitialize]
        public void Setup()
        {
            _downloader = A.Fake<IStreamDownloader>();
            A.CallTo(() => _downloader.Download("https://api.svt.se/video/ch-svt1")).Returns(_channelInfoExample);
            A.CallTo(() => _downloader.Download("https://svt1-b.akamaized.net/se/svt1/master.m3u8")).Returns(_streamInfoExample);
            _underTest = new LiveStreamManager(SourceType.Svt1Live, _downloader);
        }

        [TestMethod]
        public void ShouldDetectCorrectNumberOfStreams()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual(7, _underTest.Streams.Count); 
        }

        [TestMethod]
        public void ShouldParseAllData()
        {
            _underTest.DownloadAndParseData();
            var best = _underTest.Streams.Last();

            Assert.AreEqual(2892000, best.Bandwidth);
            Assert.AreEqual("1280x720", best.Resolution);
            Assert.AreEqual("https://svt1-b.akamaized.net/se/svt1/36603052-54ff-4835-a041-a1c25feee9a1/hls-v6/v6.m3u8", best.Url);
        }
    }
}
