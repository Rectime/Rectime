﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RecTimeLogic;

namespace RecTime
{
    class ChannelInfoBackgroundWorker : BackgroundWorker
    {
        public DateTime Date { get; set; }
        public ChannelInfo Info { get; set; }

        private SourceType _type;
        private const string DbUrl = "http://xmltv.xmltv.se/";
        
        public ChannelInfoBackgroundWorker(string channel, DateTime date)
        {
            Date = date;
            Info = new ChannelInfo();

            switch (channel)
            {
                case "Barnkanalen": _type = SourceType.SvtBarnLive; break;
                case "Kunskapskanalen": _type = SourceType.SvtKunskapLive; break;
                case "Svt1": _type = SourceType.Svt1Live; break;
                case "Svt2": _type = SourceType.Svt2Live; break;
                case "Svt24": _type = SourceType.Svt24Live; break;
            }

            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;
            this.DoWork += ChannelInfoBackgroundWorker_DoWork;
        }

        private void ChannelInfoBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var fileInfo = ChannelInfoFile.FromSourceTypeAndDate(_type, Date);
            var loader = new StreamDownloader();

            WebClient wc = new WebClient();
            MemoryStream stream = new MemoryStream(wc.DownloadData(DbUrl + fileInfo.FileName));
            var rawData = stream.ToArray();

            if (IsGZipHeader(rawData))
            {
                GZipStream uncompressed = new GZipStream(stream, CompressionMode.Decompress);
                MemoryStream output = new MemoryStream();
                uncompressed.CopyTo(output);

                var data = output.ToArray();
                var xmlString = System.Text.Encoding.UTF8.GetString(data);
                Info = StringXmlSerializer.Deserialize<ChannelInfo>(xmlString);
            }
            else
            {
                wc.Encoding = System.Text.Encoding.UTF8;
                var xmlText = wc.DownloadString(DbUrl + fileInfo.FileName);
                Info = StringXmlSerializer.Deserialize<ChannelInfo>(xmlText);
            }

            Info.Type = _type;
        }

        public static bool IsGZipHeader(byte[] arr)
        {
            return arr.Length >= 2 &&
                arr[0] == 31 &&
                arr[1] == 139;
        }
    }
}
