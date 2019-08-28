using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecTimeLogic.Tests
{
    [TestClass]
    public class Tv4StreamManagerTests
    {
        private Tv4StreamManager _underTest;

        [TestInitialize]
        public void Setup()
        {
            _underTest = new Tv4StreamManager("https://www.tv4play.se/program/bagges-heml%C3%B6sa-hundar/11794320", new StreamDownloader());
        }

        [TestMethod]
        public void TestItAll()
        {
            _underTest.DownloadAndParseData();

            Assert.AreEqual("Bagges hemlösa hundar del 1", _underTest.Title);
            Assert.AreEqual(1, _underTest.Streams.Count);
        }
    }
}
