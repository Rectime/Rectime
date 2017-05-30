using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecTimeLogic.Tests
{
    [TestClass]
    public class FfmpegInfoParserTests
    {
        private string _line1 = "frame=   60 fps=9.7 q=-1.0 size=     563kB time=00:00:02.43 bitrate=1896.9kbits/s speed=0.395x";
        private string _line2 = "frame=10786 fps=135 q=-1.0 size=  147379kB time=00:07:11.40 bitrate=2798.6kbits/s speed=5.38x ";
        private string _line3 = "frame=  512 fps= 83 q=-1.0 size=    7203kB time=00:00:20.48 bitrate=2881.1kbits/s speed=3.32x  ";
        private string _line4 = "frame= 4825 fps=135 q=-1.0 size=   66103kB time=00:03:13.02 bitrate=2805.4kbits/s speed= 5.4x";
        private string _line5 = "frame= 1682 fps= 50 q=-1.0 size=    2275kB time=00:00:56.00 bitrate= 332.8kbits/s speed=1.67x";
        private string _line6 = "size=   13031kB time=00:14:09.96 bitrate= 125.6kbits/s speed=3.31x ";

        private string _output = @"[h264 @ 06cae780] non-existing SPS 0 referenced in buffering period
[h264 @ 06cae780] SPS unavailable in decode_picture_timing
Input #0, hls,applehttp, from 'http://svtplay3l-f.akamaihd.net/i/world/open/20160914/1373860-003A/PG-1373860-003A-MAUNDERBARTMED-01_,988,240,348,456,636,1680,2796,.mp4.csmil/master.m3u8':
  Duration: 00:13:30.12, start: 10.099667, bitrate: 0 kb/s
  Program 0
    Metadata:
      variant_bitrate : 986000";

        [TestMethod]
        public void ShouldParseLine1()
        {
            var data = FFmpegInfoParser.Parse(_line1);

            Assert.IsNotNull(data);
            Assert.AreEqual(60, data.Frame);
            Assert.AreEqual(9.7, data.Fps);
            Assert.AreEqual(563, data.Size);
            Assert.AreEqual("00:00:02.43", data.Time);
            Assert.AreEqual(1896.9, data.Bitrate);
            Assert.AreEqual(0.395, data.Speed);
        }

        [TestMethod]
        public void ShouldParseLine2()
        {
            var data = FFmpegInfoParser.Parse(_line2);

            Assert.IsNotNull(data);
            Assert.AreEqual(10786, data.Frame);
            Assert.AreEqual(135, data.Fps);
            Assert.AreEqual(147379, data.Size);
            Assert.AreEqual("00:07:11.40", data.Time);
            Assert.AreEqual(2798.6, data.Bitrate);
            Assert.AreEqual(5.38, data.Speed);
        }

        [TestMethod]
        public void ShouldParseLine3()
        {
            var data = FFmpegInfoParser.Parse(_line3);

            Assert.IsNotNull(data);
            Assert.AreEqual(512, data.Frame);
            Assert.AreEqual(83, data.Fps);
            Assert.AreEqual(7203, data.Size);
            Assert.AreEqual("00:00:20.48", data.Time);
            Assert.AreEqual(2881.1, data.Bitrate);
            Assert.AreEqual(3.32, data.Speed);
        }

        [TestMethod]
        public void ShouldParseLine4()
        {
            var data = FFmpegInfoParser.Parse(_line4);

            Assert.IsNotNull(data);
            Assert.AreEqual(4825, data.Frame);
            Assert.AreEqual(135, data.Fps);
            Assert.AreEqual(66103, data.Size);
            Assert.AreEqual("00:03:13.02", data.Time);
            Assert.AreEqual(2805.4, data.Bitrate);
            Assert.AreEqual(5.4, data.Speed);
        }

        [TestMethod]
        public void ShouldParseLine5()
        {
            var data = FFmpegInfoParser.Parse(_line5);

            Assert.IsNotNull(data);
            Assert.AreEqual(1682, data.Frame);
            Assert.AreEqual(50, data.Fps);
            Assert.AreEqual(2275, data.Size);
            Assert.AreEqual("00:00:56.00", data.Time);
            Assert.AreEqual(332.8, data.Bitrate);
            Assert.AreEqual(1.67, data.Speed);
        }

        [TestMethod]
        public void ShouldParseLine6()
        {
            var data = FFmpegInfoParser.Parse(_line6);

            Assert.IsNotNull(data);
            Assert.AreEqual(0, data.Frame);
            Assert.AreEqual(0, data.Fps);
            Assert.AreEqual(13031, data.Size);
            Assert.AreEqual("00:14:09.96", data.Time);
            Assert.AreEqual(125.6, data.Bitrate);
            Assert.AreEqual(3.31, data.Speed);
        }

        [TestMethod]
        public void ShouldDetectDuration()
        {
            var data = FFmpegInfoParser.Parse(_output);

            Assert.IsNotNull(data);
            Assert.AreEqual("00:13:30.12", data.Duration);
        }
    }
}
