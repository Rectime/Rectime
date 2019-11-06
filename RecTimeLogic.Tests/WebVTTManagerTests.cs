using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecTimeLogic.Tests
{
    [TestClass]
    public class WebVTTManagerTests
    {
        private WebVTTManager _underTest;

        [TestInitialize]
        public void Setup()
        {
            _underTest = new WebVTTManager("https://lbs-usp-hls-vod.cmore.se/vod/219f6/0tosbt4rp2j(12490263_ISMUSP).ism/0tosbt4rp2j(12490263_ISMUSP)-textstream_swe=2000.m3u8", new StreamDownloader());
        }

        [TestMethod]
        public void TestItAll()
        {
            _underTest.DownloadAndProcess();

        }
    }
}
