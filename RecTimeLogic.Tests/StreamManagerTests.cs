using System;
using System.Linq;
using System.Runtime.InteropServices;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecTimeLogic.Tests
{
    [TestClass]
    public class StreamManagerTests
    {
        private string _masterDataRelativePath = @"#EXTM3U
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=985000,RESOLUTION=768x432,CODECS=""avc1.77.30, mp4a.40.2""
index_0_av.m3u8? e = 82d461566017da20&
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=238000,RESOLUTION=512x288,CODECS=""avc1.66.30, mp4a.40.2""
index_1_av.m3u8? e = 82d461566017da20&
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=346000,RESOLUTION=512x288,CODECS=""avc1.66.30, mp4a.40.2""
index_2_av.m3u8? e = 82d461566017da20&
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=454000,RESOLUTION=512x288,CODECS=""avc1.66.30, mp4a.40.2""
index_3_av.m3u8? e = 82d461566017da20&
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=634000,RESOLUTION=512x288,CODECS=""avc1.66.30, mp4a.40.2""
index_4_av.m3u8? e = 82d461566017da20&
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=1676000,RESOLUTION=1280x720,CODECS=""avc1.77.30, mp4a.40.2""
index_5_av.m3u8? e = 82d461566017da20&
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=2790000,RESOLUTION=1280x720,CODECS=""avc1.64001f, mp4a.40.2""
index_6_av.m3u8? e = 82d461566017da20&
";

        private string _masterDataAbsolutePath = @"#EXTM3U
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=985000,RESOLUTION=768x432,CODECS=""avc1.77.30, mp4a.40.2""
http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/index_0_av.m3u8?null=0
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=238000,RESOLUTION=512x288,CODECS=""avc1.66.30, mp4a.40.2""
http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/index_1_av.m3u8?null=0
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=345000,RESOLUTION=512x288,CODECS=""avc1.66.30, mp4a.40.2""
http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/index_2_av.m3u8?null=0
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=454000,RESOLUTION=512x288,CODECS=""avc1.66.30, mp4a.40.2""
http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/index_3_av.m3u8?null=0
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=634000,RESOLUTION=512x288,CODECS=""avc1.66.30, mp4a.40.2""
http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/index_4_av.m3u8?null=0
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=1676000,RESOLUTION=1280x720,CODECS=""avc1.77.30, mp4a.40.2""
http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/index_5_av.m3u8?null=0
#EXT-X-STREAM-INF:PROGRAM-ID=1,BANDWIDTH=2792000,RESOLUTION=1280x720,CODECS=""avc1.64001f, mp4a.40.2""
http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/index_6_av.m3u8?null=0";

        private string _dataAbsolutePath = @"{""videoId"":1276539,""video"":{""videoReferences"":[{""url"":""http://svtplay3p-f.akamaihd.net/z/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/manifest.f4m"",""bitrate"":0,""playerType"":""flash""},{""url"":""http://svtplay3i-f.akamaihd.net/d/se/open/delivery/20160824/1332829-001A/dash-live/PG-1332829-001A-OLLYTHELITTLE-01-1a894f3d-e7aa-336d-472e-56993a27cd04-live.mpd?alt=http%3A%2F%2Fswitcher.cdn.svt.se%2F1332829-001A.mpd"",""bitrate"":0,""playerType"":""dashhbbtv""},{""url"":""http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/master.m3u8?alt=http%3A%2F%2Fswitcher.cdn.svt.se%2F1332829-001A.m3u8&cc1=name=Svenska~default=yes~forced=no~uri=http://media.svt.se/download/mcc/test/core-prd/SUB-1332829-001A-OLLYTHELITTLE/index.m3u8~lang=sv"",""bitrate"":0,""playerType"":""ios""}],""subtitleReferences"":[{""url"":""http://media.svt.se/download/mcc/test/core-prd/SUB-1332829-001A-OLLYTHELITTLE/SUB-1332829-001A-OLLYTHELITTLE.wsrt""}],""position"":0,""materialLength"":305,""live"":false,""availableOnMobile"":false},""statistics"":{""client"":""svt-play"",""mmsClientNr"":""1001001"",""context"":""svt-play"",""programId"":""1332829-01"",""mmsCategory"":""1"",""broadcastDate"":""20160826"",""broadcastTime"":""1820"",""category"":""barn"",""statisticsUrl"":""http://ld.svt.se/svt/svt/s?svt-play.barn.sk%c3%a5pbilen-olle.hela-program.avsnitt-1"",""title"":""avsnitt-1"",""folderStructure"":""skåpbilen-olle.hela-program""},""context"":{""title"":""Avsnitt 1"",""programTitle"":""Skåpbilen Olle"",""thumbnailImage"":""http://www.svtstatic.se/image-cms/barn/1452437040/incoming/article1276538.svt/ALTERNATES/extralarge/default_title"",""posterImage"":""http://www.svtstatic.se/image-cms/barn/1370609760/incoming/article1275500.svt/ALTERNATES/extralarge/skapbilenolle480.png"",""popoutUrl"":""/video/1276539/skapbilen-olle/avsnitt-1?type=embed"",""programVersionId"":""1332829-001A""}}";
        private string _dataRelativePath = @"{""videoId"":9169569,""video"":{""videoReferences"":[{""url"":""http://svtplay13n-f.akamaihd.net/d/world/open/delivery/20160613/1371702-001A/dash-ondemand/PG-1371702-001A-BON2016-02-6f0049ca-62ee-336d-9bc0-716bd4b5cd08-ondemand.mpd?alt=http%3A%2F%2Fswitcher.cdn.svt.se%2F1371702-001A.mpd"",""bitrate"":0,""playerType"":""dash264""},{""url"":""http://svtplay13a-f.akamaihd.net/i/world/open/20160613/1371702-001A/PG-1371702-001A-BON2016-02_,988,240,348,456,636,1680,2796,.mp4.csmil/master.m3u8?alt=http%3A%2F%2Fswitcher.cdn.svt.se%2F1371702-001A.m3u8&cc1=name=Svenska~default=yes~forced=no~uri=http://media.svt.se/download/mcc/test/core-prd/SUB-1371702-001A-BON/index.m3u8~lang=sv"",""bitrate"":0,""playerType"":""ios""},{""url"":""http://svtplay13b-f.akamaihd.net/d/world/open/delivery/20160613/1371702-001A/dash-live/PG-1371702-001A-BON2016-02-6f0049ca-62ee-336d-36fe-4c5c75fc1837-live.mpd?alt=http%3A%2F%2Fswitcher.cdn.svt.se%2F1371702-001A.mpd"",""bitrate"":0,""playerType"":""dashhbbtv""},{""url"":""http://svtplay13p-f.akamaihd.net/z/world/open/20160613/1371702-001A/PG-1371702-001A-BON2016-02_,988,240,348,456,636,1680,2796,.mp4.csmil/manifest.f4m"",""bitrate"":0,""playerType"":""flash""}],""subtitleReferences"":[{""url"":""http://media.svt.se/download/mcc/test/core-prd/SUB-1371702-001A-BON/index.m3u8""}],""position"":0,""materialLength"":179,""live"":false,""availableOnMobile"":true},""statistics"":{""client"":""svt-play"",""mmsClientNr"":""1001001"",""context"":""svt-play"",""programId"":""1371702-01"",""mmsCategory"":""1"",""broadcastDate"":""20160614"",""broadcastTime"":""0903"",""category"":""barn"",""statisticsUrl"":""http://ld.svt.se/svt/svt/s?svt-play.barn.bon.hela-program.sasong-2-avsnitt-1"",""title"":""sasong-2-avsnitt-1"",""folderStructure"":""bon.hela-program""},""context"":{""title"":""Avsnitt 1"",""programTitle"":""Bon"",""thumbnailImage"":""http://www.svtstatic.se/image-cms/barn/1465886796/bon/article9169557.svt/ALTERNATES/extralarge/bon-s-02-e01-01-1024-jpg"",""posterImage"":""http://www.svtstatic.se/image-cms/barn/1466596327/bon/article2471257.svt/ALTERNATES/extralarge/bon-s2-barnkanalen-480-jpg"",""popoutUrl"":""/video/9169569/bon/bon-sasong-2-avsnitt-1?type=embed"",""programVersionId"":""1371702-001A""}}";

        private StreamManager _underTest;
        private string _stream0DataUrl = "http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/index_0_av.m3u8";
        private string _stream6DataUrl = "http://svtplay13a-f.akamaihd.net/i/world/open/20160613/1371702-001A/PG-1371702-001A-BON2016-02_,988,240,348,456,636,1680,2796,.mp4.csmil/index_6_av.m3u8";
        private string _htmlUri = "http://www.svtplay.se/video/1276539/skapbilen-olle/avsnitt-1";
        private string _dataUrl = "http://www.svtplay.se/video/1276539?output=json";
        private string _streamMasterDataUrl = "http://svtplay3p-f.akamaihd.net/i/se/open/20160824/1332829-001A/PG-1332829-001A-OLLYTHELITTLE-01_,988,240,348,456,636,1680,2796,.mp4.csmil/master.m3u8";
        private string _streamMasterDataUrlRelative = "http://svtplay13a-f.akamaihd.net/i/world/open/20160613/1371702-001A/PG-1371702-001A-BON2016-02_,988,240,348,456,636,1680,2796,.mp4.csmil/master.m3u8";
        private IStreamDownloader _downloader;

        [TestInitialize]
        public void Setup()
        {
            _downloader = A.Fake<IStreamDownloader>();
            A.CallTo(() => _downloader.Download(_dataUrl)).Returns(_dataAbsolutePath);
            A.CallTo(() => _downloader.Download(_streamMasterDataUrl)).Returns(_masterDataAbsolutePath);
            _underTest = new StreamManager(_htmlUri, _downloader);
        }

        [TestMethod]
        public void ShouldParseCorrectNumberOfStreams()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual(7, _underTest.Streams.Count);
        }

        [TestMethod]
        public void ShouldParseCorrectResolution()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual("768x432", _underTest.Streams.First().Resolution);
        }

        [TestMethod]
        public void ShouldParseCorrectTitle()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual("avsnitt-1", _underTest.Title);
        }

        [TestMethod]
        public void ShouldParseCorrectProgramTitle()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual("Skåpbilen Olle", _underTest.ProgramTitle);
        }

        [TestMethod]
        public void ShouldParseCorrectPosterUrl()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual("http://www.svtstatic.se/image-cms/barn/1452437040/incoming/article1276538.svt/ALTERNATES/extralarge/default_title", _underTest.PosterUrl);
        }

        [TestMethod]
        public void ShouldParseCorrectDuration()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual(305, _underTest.Duration);
        }

        [TestMethod]
        public void ShouldParseCorrectCodec()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual("avc1.77.30, mp4a.40.2", _underTest.Streams.First().Codec);
        }

        [TestMethod]
        public void ShouldParseCorrectUrl()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual(_stream0DataUrl, _underTest.Streams.First().Url);
        }

        [TestMethod]
        public void ShouldParseCorrectBandwidth()
        {
            _underTest.DownloadAndParseData();
            Assert.AreEqual(985000, _underTest.Streams.First().Bandwidth);
        }

        [TestMethod]
        public void ShouldDownLoadStreamInfo()
        {
            var manager = new StreamManager(_htmlUri, new StreamDownloader());
            manager.DownloadAndParseData();
            Assert.AreEqual(7, manager.Streams.Count);
        }

        [TestMethod]
        public void ShouldDownloadAndParseDataWithCorrectStreamInfoUrl()
        {
            //var manager = new StreamManager(_htmlUri, new StreamDownloader());
            //manager.DownloadAndParseData();
            //Assert.AreEqual(_stream0DataUrl, manager.Streams.First().Url);
        }

        [TestMethod]
        public void ShouldDetectCorrectDataUrl()
        {
            var manager = new StreamManager(_htmlUri, new StreamDownloader());
            manager.DownloadAndParseData();
            Assert.AreEqual(_dataUrl, manager.DataUrl);
        }

        [TestMethod]
        public void ShouldHandleRelativeStreamUrls()
        {
            var downloader = A.Fake<IStreamDownloader>();
            A.CallTo(() => downloader.Download(_dataUrl)).Returns(_dataRelativePath);
            A.CallTo(() => downloader.Download(_streamMasterDataUrlRelative)).Returns(_masterDataRelativePath);
            var manager = new StreamManager(_htmlUri, downloader);

            manager.DownloadAndParseData();

            Assert.AreEqual(_stream6DataUrl, manager.Streams.Last().Url);
        }

    }
}
