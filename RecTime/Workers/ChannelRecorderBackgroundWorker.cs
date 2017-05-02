using System;
using System.ComponentModel;
using System.Linq;
using RecTimeLogic;

namespace RecTime
{
    class ChannelRecorderBackgroundWorker : StreamBackgroundWorker
    {
        public ProgramInfo Info { get; private set; }
        public override bool IsChannelRecorder => true;
        public SourceType Type { get; private set; }


        public ChannelRecorderBackgroundWorker(SourceType type, ProgramInfo info, string outputDirectory) 
            : base(null, outputDirectory, type + info.Filename)
        {
            Info = info;
            Type = type;
        }

        protected override void DownloadStream(object sender, DoWorkEventArgs e)
        {
            var manager = new LiveStreamManager(Type, new StreamDownloader());
            manager.DownloadAndParseData();
            StreamInfo = manager.Streams.Last();

            TimeLimit = Info.StopTime - DateTime.Now;
            Duration = TimeLimit.Value.ToString();

            base.DownloadStream(sender, e);
        }
    }
}
