using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecTimeLogic.Tests
{
    [TestClass]
    public class ChannelInfoFileTests
    {
        [TestMethod]
        public void ShouldReturnCorrectFilenameForSvt1()
        {
            var fileInfo = ChannelInfoFile.FromSourceTypeAndDate(SourceType.Svt1Live, new DateTime(2017, 4, 20));
            
            Assert.AreEqual(SourceType.Svt1Live, fileInfo.Source);
            Assert.AreEqual("svt1.svt.se_2017-04-20.xml.gz", fileInfo.FileName);
        }

        [TestMethod]
        public void ShouldReturnCorrectFilenameForSvt2()
        {
            var fileInfo = ChannelInfoFile.FromSourceTypeAndDate(SourceType.Svt2Live, new DateTime(2017, 4, 20));

            Assert.AreEqual(SourceType.Svt2Live, fileInfo.Source);
            Assert.AreEqual("svt2.svt.se_2017-04-20.xml.gz", fileInfo.FileName);
        }

        [TestMethod]
        public void ShouldReturnCorrectFilenameForSvt24()
        {
            var fileInfo = ChannelInfoFile.FromSourceTypeAndDate(SourceType.Svt24Live, new DateTime(2017, 4, 20));

            Assert.AreEqual(SourceType.Svt24Live, fileInfo.Source);
            Assert.AreEqual("svt24.svt.se_2017-04-20.xml.gz", fileInfo.FileName);
        }

        [TestMethod]
        public void ShouldReturnCorrectFilenameForSvtB()
        {
            var fileInfo = ChannelInfoFile.FromSourceTypeAndDate(SourceType.SvtBarnLive, new DateTime(2017, 4, 20));

            Assert.AreEqual(SourceType.SvtBarnLive, fileInfo.Source);
            Assert.AreEqual("svtb.svt.se_2017-04-20.xml.gz", fileInfo.FileName);
        }

        [TestMethod]
        public void ShouldReturnCorrectFilenameForKunskapskanalen()
        {
            var fileInfo = ChannelInfoFile.FromSourceTypeAndDate(SourceType.SvtKunskapLive, new DateTime(2017, 4, 20));

            Assert.AreEqual(SourceType.SvtKunskapLive, fileInfo.Source);
            Assert.AreEqual("kunskapskanalen.svt.se_2017-04-20.xml.gz", fileInfo.FileName);
        }
    }
}
