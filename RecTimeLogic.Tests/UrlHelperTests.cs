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
    }
}
