using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecTimeLogic.Tests
{
    [TestClass]
    public class ChannelInfoTests
    {
        private ChannelInfo underTest;

        [TestInitialize]
        public void Setup()
        {
            var fileInfo = ChannelInfoFile.FromSourceTypeAndDate(SourceType.Svt1Live,
                new DateTime(2017, 4, 20));
            var data = File.ReadAllText("ChannelData/" + fileInfo.FileName);
            underTest =
                StringXmlSerializer.Deserialize<ChannelInfo>(data);
        }

        [TestMethod]
        public void ShouldParseAllPrograms()
        {
            Assert.AreEqual(31, underTest.Programs.Count);
        }

        [TestMethod]
        public void ShouldParseProgramTitle()
        {
            Assert.AreEqual("Engelska Antikrundan", underTest.Programs.First().Title);
        }
    }
}
