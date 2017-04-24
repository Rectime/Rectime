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
            var data = File.ReadAllBytes("ChannelData/" + fileInfo.FileName);
            Encoding ansi = Encoding.GetEncoding(1252);
            var s = ansi.GetString(data);
            underTest =
                StringXmlSerializer.Deserialize<ChannelInfo>(s);
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

        [TestMethod]
        public void ShouldParseProgramDescription()
        {
            Assert.AreEqual("Bodnant Garden. Programledaren Fiona Bruce och antikexperterna åker runt Storbritannien på jakt efter gömda skatter i ett av de mest populära faktaprogrammen som har fängslat den engelska publiken i mer än 30 år. Från 19/4 i SVT2.", 
                underTest.Programs.First().Description);
        }

        [TestMethod]
        public void ShouldParseProgramCategory()
        {
            Assert.AreEqual("Series",
                underTest.Programs.First().Category);
        }

        [TestMethod]
        public void ShouldParseProgramStart()
        {
            Assert.AreEqual("20170420100000 +0200",
                underTest.Programs.First().Start);
        }

        [TestMethod]
        public void ShouldParseProgramStop()
        {
            Assert.AreEqual("20170420110000 +0200",
                underTest.Programs.First().Stop);
        }

        [TestMethod]
        public void ShouldInterpretStartTime()
        {
            Assert.AreEqual(new DateTime(2017, 4, 20, 10, 0, 0), 
                underTest.Programs.First().StartTime);
        }

        [TestMethod]
        public void ShouldInterpretStopTime()
        {
            Assert.AreEqual(new DateTime(2017, 4, 20, 11, 0, 0),
                underTest.Programs.First().StopTime);
        }

        [TestMethod]
        public void ShouldFormatTimeAndTitle()
        {
            Assert.AreEqual("10.00-11.00: Engelska Antikrundan",
                underTest.Programs.First().TimeAndTitle);
        }
    }
}
