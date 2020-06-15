using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using RecTimeLogic;
using AutoUpdaterDotNET;

namespace RecTime
{
    public partial class RecTime : MaterialForm
    {
        private InfoBackgroundWorker _infoWorker;
        private GoogleAnalyticsTracker _tracker;
        private StreamManager _infoResult;
        private int _previousLength = 0;
        private List<StreamBackgroundWorker> _workers = new List<StreamBackgroundWorker>(); 
        private Dictionary<MaterialRadioButton, StreamInfo> _streamButtons = new Dictionary<MaterialRadioButton, StreamInfo>();
        private bool _closing = false;
        private List<LiveChannelPreview> _channelPreviews = new List<LiveChannelPreview>();
        private List<Button> _channelCloseButtons = new List<Button>();
        private List<Button> _channelRecButtons = new List<Button>();

        #region Constructor 

        public RecTime()
        {
            InitializeComponent();
            Application.EnableVisualStyles();
            this.DoubleBuffered = true;

            this.StartPosition = FormStartPosition.CenterScreen;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);

            string path = (Directory.Exists(Properties.Settings.Default.DownloadPath)) ?
                Properties.Settings.Default.DownloadPath : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            txtOutputLocation.Text = path;
            Properties.Settings.Default.DownloadPath = path;
            Properties.Settings.Default.Save();

            if (Properties.Settings.Default.UserId == Guid.Empty)
            {
                Properties.Settings.Default.UserId = Guid.NewGuid();
                Properties.Settings.Default.Save();
            }
            _tracker = new GoogleAnalyticsTracker(Application.ProductVersion, 
                Properties.Settings.Default.UserId.ToString());

            _channelPreviews.Add(new LiveChannelPreview(pictureBoxSvt1, SourceType.Svt1Live));
            _channelPreviews.Add(new LiveChannelPreview(pictureBoxSvt2, SourceType.Svt2Live));
            _channelPreviews.Add(new LiveChannelPreview(pictureBoxSvt24, SourceType.Svt24Live));
            _channelPreviews.Add(new LiveChannelPreview(pictureBoxBarn, SourceType.SvtBarnLive));
            _channelPreviews.Add(new LiveChannelPreview(pictureBoxKunskap, SourceType.SvtKunskapLive));

            _channelCloseButtons.AddRange(new[] { btnChannelClose1, btnChannelClose2, btnChannelClose24, btnChannelCloseBarn, btnChannelCloseKunskap});
            _channelRecButtons.AddRange(new[] { btnChannelRec1, btnChannelRec2, btnChannelRec24, btnChannelRecBarn, btnChannelRecKunskap});
        }

        #endregion

        #region Component Events

        private void numericLiveStartOffset_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LiveStartOffset = (int)numericLiveStartOffset.Value;
            Properties.Settings.Default.Save();
        }

        private void numericLiveStopOffset_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.LiveStopOffset = (int)numericLiveStopOffset.Value;
            Properties.Settings.Default.Save();
        }

        private void timerChannels_Tick(object sender, EventArgs e)
        {
            int startOffset = Properties.Settings.Default.LiveStartOffset;
            int stopOffset = Properties.Settings.Default.LiveStartOffset;

            _workers.OfType<ChannelRecorderBackgroundWorker>().Where(w => !w.IsBusy && !w.HasRun && 
                (w.Info.StartTime + TimeSpan.FromSeconds(startOffset)) <= DateTime.Now && (w.Info.StopTime + TimeSpan.FromSeconds(stopOffset)) >= DateTime.Now)
                .ToList().ForEach(w => w.StartAndUpdateOffsetTime(stopOffset));

            _workers.OfType<ChannelRecorderBackgroundWorker>().Where(w => (w.Info.StartTime + TimeSpan.FromSeconds(startOffset)) > DateTime.Now).
                ToList().ForEach(w => UpdateQueueStatus(w, "om " + (w.Info.StartTime + TimeSpan.FromSeconds(startOffset) - DateTime.Now).ToString(@"hh\:mm\:ss")));
        }

        private void btnChannel_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null)
                return;

            var form = new ChannelForm() {Channel = btn.Tag.ToString()};
            _workers.OfType<ChannelRecorderBackgroundWorker>().ToList().ForEach(w => form.QueuedPrograms.Add(w.Info));

            form.FormClosed += Form_FormClosed;
            form.Show();
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            var form = sender as ChannelForm;
            form.FormClosed -= Form_FormClosed;

            if (form.DialogResult == DialogResult.OK)
            {
                foreach (var program in form.SelectedPrograms)
                {
                    _tracker.SendEvent("Download " + form.Type, program.Title, null, 0);
                    var worker = new ChannelRecorderBackgroundWorker(form.Type, program, txtOutputLocation.Text);
                    
                    var filename = txtOutputLocation.Text + @"\" + worker.OutputFilename;
                    if (File.Exists(filename))
                    {
                        MessageBox.Show(this, "Filen finns redan: " + worker.OutputFilename, "Konflikt", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                            continue;
                    }

                    //no dupes
                    if(_workers.OfType<ChannelRecorderBackgroundWorker>().Any(w => w.Info.Equals(program)))
                        continue;
                    _workers.Add(worker);

                    var data = new[] { program.Title, form.Type.ToString(), "0 %" };
                    var item = new ListViewItem(data) { Tag = worker };
                    listViewQueue.Items.Add(item);

                    worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                    worker.ProgressChanged += Worker_ProgressChanged;
                }
            }
        }

        private void txtOutputLocation_Enter(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtOutputLocation.Text = folderBrowserDialog.SelectedPath;
                Properties.Settings.Default.DownloadPath = txtOutputLocation.Text;
                Properties.Settings.Default.Save();
            }
        }

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            _infoResult = null;
            btnStartDownload.Visible = false;

            foreach (var kvp in _streamButtons)
            {
                kvp.Key.CheckedChanged -= Radio_CheckedChanged;
                panelStreams.Controls.Remove(kvp.Key);
            }
            _streamButtons.Clear();
            pictureBox.Image = null;

            bool validUrl = IsValidUrl(txtUrl.Text);

            if (Math.Abs(txtUrl.TextLength - _previousLength) > 1 && validUrl)
            {
                _infoWorker = new InfoBackgroundWorker(txtUrl.Text);
                _infoWorker.RunWorkerCompleted += _infoWorker_RunWorkerCompleted;
                _infoWorker.RunWorkerAsync();
            }
            _previousLength = txtUrl.TextLength;
        }

        private void btnStartDownload_Click(object sender, EventArgs e)
        {
            if (_infoResult != null)
            {
                _tracker.SendEvent("Download " +_infoResult.Type, _infoResult.LongTitle, _infoResult.BaseUrl, _infoResult.Duration);

                var filename = txtOutputLocation.Text + @"\" + txtFilename.Text;
                if (File.Exists(filename))
                {
                    MessageBox.Show(this, "File already exists", "File exists", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                var info = _streamButtons.First(b => b.Key.Checked).Value;
                info.SubtitleUrl = (checkBoxSubs.Checked) ? info.SubtitleUrl : null;
                var worker = new StreamBackgroundWorker(info, txtOutputLocation.Text, txtFilename.Text, Properties.Settings.Default.ConvertVTTtoSRT);
                _workers.Add(worker);

                //Define
                var title = _infoResult.Title.Length > 50 ? _infoResult.Title.Substring(0, 50) + "..." : _infoResult.Title;
                var data = new []{title, _infoResult.Type.ToString(), "0 %" };
                var item = new ListViewItem(data) { Tag = worker };
                listViewQueue.Items.Add(item);
                

                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                worker.ProgressChanged += Worker_ProgressChanged;

                if(!_workers.Any(w => w.IsBusy))
                    worker.RunWorkerAsync();
            }
        }

        private void materialTabControl1_Selected(object sender, TabControlEventArgs e)
        {
            _tracker.SendView(e.TabPage.Text);
        }

        private void txtUrl_Enter(object sender, EventArgs e)
        {
            txtUrl.Clear();
        }

        #endregion

        #region Worker events

        private void _infoWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _infoResult = _infoWorker.Manager;
            _infoWorker.RunWorkerCompleted -= _infoWorker_RunWorkerCompleted;
            _infoWorker = null;

            int i = 0;
            foreach (var streamInfo in _infoResult.Streams)
            {
                var radio = new MaterialRadioButton();
                radio.Text = streamInfo.ToString();
                radio.Size = new Size(270, 18);
                radio.Location = new Point(5, 5 + i * 20);
                radio.CheckedChanged += Radio_CheckedChanged;
                panelStreams.Controls.Add(radio);
                i++;
                _streamButtons.Add(radio, streamInfo);
            }

            if (_streamButtons.Count > 0)
            {
                txtFilename.Text = _infoResult.GetValidFileName(_infoResult.Streams.Last());
                pictureBox.Image = _infoResult.PosterImage;
                _streamButtons.Last().Key.Checked = true;
                btnStartDownload.Visible = true;
                lblStatus.Text = $"Status: Hittade {_streamButtons.Count} strömmar";
            }
            else
            {
                lblStatus.Text = "Status: Kunde ej hitta video stream";
            }

            if(e.Error != null)
            {
                switch(e.Error)
                {
                    case DrmProtectionException drm:
                        lblStatus.Text = "Status: " + drm.Message;
                        break;
                }
            }
        }

        private void Radio_CheckedChanged(object sender, EventArgs e)
        {
            var radio = sender as MaterialRadioButton;
            if (sender == null || !_streamButtons.ContainsKey(radio) || !radio.Checked)
                return;

            txtFilename.Text = _infoResult.GetValidFileName(_streamButtons[radio]);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var infoData = e.UserState as FFmpegInfo;
            var worker = sender as StreamBackgroundWorker;

            if (infoData != null && worker != null)
            {
                TimeSpan timeNow;
                TimeSpan timeDuration;

                int workerCount = _workers.Count;
                int workerInProgress = _workers.Count(w => w.HasRun || w.IsBusy);
                var channel = worker as ChannelRecorderBackgroundWorker;

                if (TimeSpan.TryParse(infoData.Time, out timeNow) &&
                    TimeSpan.TryParse(worker.Duration, out timeDuration) && timeDuration.TotalSeconds > 0)
                {
                    string percentage = $"{(timeNow.TotalSeconds / timeDuration.TotalSeconds):0.0 %}";
                    lblStatus.Text = $"Status: {percentage,6} {infoData} ({workerInProgress}/{workerCount})";
                    UpdateQueueStatus(worker, percentage);
                }
                else
                    lblStatus.Text = $"Status: {infoData} ({workerInProgress}/{workerCount})";
            }

            var info = e.UserState as string;
            if (!string.IsNullOrEmpty(info))
                lblStatus.Text = $"Status: {info}";
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_closing)
                return;

            ListViewItem queueItem = null;
            foreach (ListViewItem item in listViewQueue.Items)
            {
                if (item.Tag == sender)
                    queueItem = item;
            }

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.ToString());
                if(queueItem != null)
                    queueItem.SubItems[2].Text = "Error";
            }
            else
            {
                if (queueItem != null)
                    queueItem.SubItems[2].Text = "Done";
            }

            lblStatus.Text = "Status: Done";

            var worker = _workers.FirstOrDefault(w => !w.IsBusy && !w.HasRun);
            worker?.RunWorkerAsync();
        }

        #endregion

        #region Form events

        private void RecTime_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_workers.Any(w => w.IsBusy) || _workers.OfType<ChannelRecorderBackgroundWorker>().Any(w => !w.HasRun))
            {
                if (
                    MessageBox.Show(this, "Du har pågående nedladdningar eller köade nedladdningar som inte är klara.\n\r\n\rVill du avsluta?" , "Ej klar", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            _closing = true;

            foreach (var liveChannelPreview in _channelPreviews)
            {
                liveChannelPreview.Kill();
            }

            foreach (var streamBackgroundWorker in _workers)
            {
                if (streamBackgroundWorker.IsBusy)
                    streamBackgroundWorker.CancelAsync();
            }
        }

        private void RecTime_Activated(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                var data = (string)Clipboard.GetData(DataFormats.Text);
                if (IsValidUrl(data) && data != txtUrl.Text)
                {
                    _previousLength = 0;
                    txtUrl.Text = data;
                }

            }
        }

        private void RecTime_Load(object sender, EventArgs e)
        {
            Random rnd = new Random();
            AutoUpdater.Start("http://rectime.se/update/latest.xml?v=" + rnd.Next(10000));
            _tracker.SendView(materialTabControl1.TabPages[0].Text);
            materialLabelVersion.Text = "v." + Application.ProductVersion;

            numericLiveStartOffset.Value = Properties.Settings.Default.LiveStartOffset;
            numericLiveStopOffset.Value = Properties.Settings.Default.LiveStopOffset;
            settingsCheckBoxVTTtoSRT.Checked = Properties.Settings.Default.ConvertVTTtoSRT;
        }

        #endregion

        #region Private Helper Methods

        private void UpdateQueueStatus(StreamBackgroundWorker worker, string text)
        {
            foreach (ListViewItem item in listViewQueue.Items)
            {
                if (item.Tag == worker)
                    item.SubItems[2].Text = text;
            }
        }

        private bool IsValidUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            Uri uriResult;
            bool validUrl = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            return (validUrl && UrlHelper.ParseUrl(url).Item1 != SourceType.Unknown);
        }

        #endregion

        #region PictureBox Events

        private void pictureBoxDonate_Click(object sender, EventArgs e)
        {
            string url = "";

            string business = "info@rectime.se";  
            string description = "Donation";          
            string country = "SE";                  // AU, US, etc.
            string currency = "SEK";                 // AUD, USD, etc.

            url += "https://www.paypal.com/cgi-bin/webscr" +
                "?cmd=" + "_donations" +
                "&business=" + business +
                "&lc=" + country +
                "&item_name=" + description +
                "&currency_code=" + currency +
                "&bn=" + "PP%2dDonationsBF";

            System.Diagnostics.Process.Start(url);
        }

        private void pictureBoxFFmpeg_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://ffmpeg.org");
        }

        private void pictureBoxChannelPreview_Click(object sender, EventArgs e)
        {
            var preview = _channelPreviews.FirstOrDefault(c => c.Parent == sender);
            preview?.Activate();

            var btnClose = _channelCloseButtons.FirstOrDefault(b => (string)b.Tag == preview.Channel.ToString());
            if (btnClose != null) btnClose.Enabled = true;
        }

        private void btnChannelClose_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            
            var preview = _channelPreviews.FirstOrDefault(c => c.Channel.ToString() == (string)btn.Tag);
            preview?.Kill();
            if (btn != null) btn.Enabled = false;
        }

        private void btnChannelRec_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            var type = (SourceType)Enum.Parse(typeof(SourceType), btn.Tag.ToString());

            var dialog = new TimeSelectForm();
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return;

            _tracker.SendEvent("Download " + type, "Live", null, 0);
            var ProgramInfo = new ProgramInfo()
            {
                Title = type.ToString(),
                Start = DateTime.Now.ToString("yyyyMMddHHmmss zzz"),
                Stop = (DateTime.Now + dialog.Duration).ToString("yyyyMMddHHmmss zzz")
            };
            var worker = new ChannelRecorderBackgroundWorker(type, ProgramInfo, txtOutputLocation.Text);
            var filename = txtOutputLocation.Text + @"\" + worker.OutputFilename;
            if (File.Exists(filename))
            {
                MessageBox.Show(this, "Filen finns redan: " + worker.OutputFilename, "Konflikt", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            //no dupes
            if (_workers.OfType<ChannelRecorderBackgroundWorker>().Any(w => w.Info.Equals(ProgramInfo)))
                return;
            _workers.Add(worker);

            var data = new[] { ProgramInfo.Title, type.ToString(), "0 %" };
            var item = new ListViewItem(data) { Tag = worker };
            listViewQueue.Items.Add(item);

            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            worker.ProgressChanged += Worker_ProgressChanged;
        }

        #endregion

        private void ListViewQueue_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listViewQueue.FocusedItem.Bounds.Contains(e.Location))
                {
                    queueContextMenuStrip.Show(Cursor.Position);
                }
            }
        }

        private void ToolStripMenuItemRemove_Click(object sender, EventArgs e)
        {
            var item = listViewQueue.FocusedItem;

            if (item == null)
                return;

            var worker = item.Tag as StreamBackgroundWorker;
            if (worker == null)
                return;

            if(!worker.IsBusy)
            {
                listViewQueue.Items.Remove(item);
                _workers.Remove(worker);
            }
            else
            {
                if (MessageBox.Show(this, "Nedladdning pågår.\n\r\n\rVill du avsluta?", "Ej klar", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning) == DialogResult.No)
                    return;

                listViewQueue.Items.Remove(item);
                worker.CancelAsync();
                _workers.Remove(worker);
            }
        }

        private void SettingsCheckBoxVTTtoSRT_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.ConvertVTTtoSRT = settingsCheckBoxVTTtoSRT.Checked;
            Properties.Settings.Default.Save();
        }
    }
}
