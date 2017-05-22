using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecTimeLogic.Tests
{
    [TestClass]
    public class UrlHelperTests
    {
        [TestMethod]
        public void ShouldDetectSvtPlay()
        {
            var result = UrlHelper.ParseUrl("http://www.svtplay.se/video/1276539/skapbilen-olle/avsnitt-1");
            Assert.AreEqual(SourceType.SvtPlay, result.Item1);
        }

        [TestMethod]
        public void ShouldParseSvtPlayDataUrl()
        {
            var result = UrlHelper.ParseUrl("http://www.svtplay.se/video/1276539/skapbilen-olle/avsnitt-1");
            Assert.AreEqual("http://www.svtplay.se/video/1276539?output=json", result.Item2);
        }

        [TestMethod]
        public void ShouldParseYouTubeDataUrl()
        {
            var url = "https://www.youtube.com/watch?v=z-LtTQflSHg";

            var result = UrlHelper.ParseUrl(url);
            Assert.AreEqual("http://www.youtube.com/get_video_info?&video_id=z-LtTQflSHg&el=detailpage&ps=default&eurl=&gl=US&hl=en", result.Item2);
        }

        [TestMethod]
        public void ShouldParseInvalidYouTubeDataUrlAsEmpty()
        {
            var url = "https://www.youtube.com/";

            var result = UrlHelper.ParseUrl(url);
            Assert.AreEqual(string.Empty, result.Item2);
        }
    }
}
