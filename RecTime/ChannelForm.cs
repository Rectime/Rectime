using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using RecTimeLogic;

namespace RecTime
{
    public partial class ChannelForm : MaterialForm
    {
        public string Channel { get; set; }
        public SourceType Type { get; private set; }
        public DateTime CurrentDate { get; set; }
        public List<ProgramInfo> SelectedPrograms { get; set; }
        public List<ProgramInfo> QueuedPrograms { get; set; }

        private ChannelInfoBackgroundWorker _worker;

        #region Constructor

        public ChannelForm()
        {
            InitializeComponent();
            SelectedPrograms = new List<ProgramInfo>();
            QueuedPrograms = new List<ProgramInfo>();
            this.StartPosition = FormStartPosition.CenterScreen;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        #endregion

        #region Form Events

        private void ChannelForm_Load(object sender, EventArgs e)
        {
            this.Text += Channel;
            CurrentDate = DateTime.Today;
            _worker = new ChannelInfoBackgroundWorker(Channel, CurrentDate);
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;

            var image = Properties.Resources.svt1;

            switch (Channel)
            {
                case "Barnkanalen": image = Properties.Resources.barnkanalen; break;
                case "Kunskapskanalen": image = Properties.Resources.kunskapskanalen; break;
                case "Svt1": image = Properties.Resources.svt1; break;
                case "Svt2": image = Properties.Resources.svt2; break;
                case "Svt24": image = Properties.Resources.svt24; break;
            }
            pictureBoxChannel.Image = image;

            StartWorker();
        }

        #endregion

        #region BackgroundWorker Events

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            foreach (Button oldButton in panelPrograms.Controls)
            {
                oldButton.MouseEnter -= Btn_MouseEnter;
                oldButton.Click -= Btn_Click;
            }
            panelPrograms.Controls.Clear();
            int i = 0;
            var font = new Font("Roboto", 11);

            foreach (var program in _worker.Info.Programs)
            {
                if(program.StopTime < DateTime.Now)
                    continue;
                var btn = new Button
                {
                    Text = program.TimeAndTitle,
                    BackColor = SelectedPrograms.Contains(program) ? Color.FromArgb(78, 100, 106) : Color.FromArgb(38, 50, 56),
                    Location = new Point(5, 15 + i * 36),
                    Size = new Size(360, 36),
                    Tag = program,
                    ForeColor = Color.White,
                    Font = font,
                    TextAlign = ContentAlignment.MiddleLeft
                };

                if (QueuedPrograms.Contains(program))
                {
                    btn.Enabled = false;
                    btn.Text += " (redan köad)";
                }
                btn.MouseEnter += Btn_MouseEnter;
                btn.Click += Btn_Click;
                panelPrograms.Controls.Add(btn);
                i++;
            }
            SetControlsEnabled(true);
        }

        #endregion

        #region Button Events

        private void Btn_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn?.Tag == null)
                return;

            ProgramInfo info = (ProgramInfo)btn.Tag;
            if (SelectedPrograms.Contains(info))
            {
                SelectedPrograms.Remove(info);
                btn.BackColor = Color.FromArgb(38, 50, 56);
            }
            else
            {
                SelectedPrograms.Add(info);
                btn.BackColor = Color.FromArgb(78, 100, 106);
            }

            if (SelectedPrograms.Count > 0)
                lblCount.Text = SelectedPrograms.Count + " valda";
            else
                lblCount.Text = string.Empty;
        }

        private void Btn_MouseEnter(object sender, EventArgs e)
        {
            var btn = sender as Button;
            if (btn?.Tag == null)
                return;

            ProgramInfo info = (ProgramInfo)btn.Tag;
            lblDescription.Text = info.Description;
        }

        private void btnPrevDate_Click(object sender, EventArgs e)
        {
            if (CurrentDate <= DateTime.Today)
                return;

            CurrentDate -= TimeSpan.FromDays(1);
            StartWorker();
        }

        private void btnNextDate_Click(object sender, EventArgs e)
        {
            CurrentDate += TimeSpan.FromDays(1);
            StartWorker();
        }

        private void btnAddSelection_Click(object sender, EventArgs e)
        {
            Type = _worker.Info.Type;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        #endregion

        #region Private Helper Methods
        private void StartWorker()
        {
            UpdateDateText();
            _worker.Date = CurrentDate;
            SetControlsEnabled(false);
            _worker.RunWorkerAsync();
        }

        private void SetControlsEnabled(bool status)
        {
            btnAddSelection.Enabled = status;
            btnNextDate.Enabled = status;
            btnPrevDate.Enabled = status;
            lblDate.Enabled = status;
            lblDescription.Enabled = status;
            lblCount.Enabled = status;
            pictureBoxLoading.Visible = !status;
        }

        private void UpdateDateText()
        {
            var today = DateTime.Today;
            var tomorrow = DateTime.Today + TimeSpan.FromDays(1);

            if (CurrentDate == today)
                lblDate.Text = "Idag";
            else if (CurrentDate == tomorrow)
                lblDate.Text = "Imorgon";
            else
                lblDate.Text = CurrentDate.ToShortDateString();
        }

        #endregion
    }
}
