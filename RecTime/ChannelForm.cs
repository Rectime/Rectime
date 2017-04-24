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

namespace RecTime
{
    public partial class ChannelForm : MaterialForm
    {
        public string Channel { get; set; }
        public DateTime CurrentDate { get; set; }

        public ChannelForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;

            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }

        private void ChannelForm_Load(object sender, EventArgs e)
        {
            this.Text += Channel;
            CurrentDate = DateTime.Today;
            UpdateDateText();

            for (int i = 0; i < 15; i++)
            {
                var btn = new MaterialRaisedButton();
                btn.Text = "10.00-13.00: Antikrundan special ...";
                btn.Location = new Point(5, 15 + i*33);
                panelPrograms.Controls.Add(btn);
            }
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


        private void btnPrevDate_Click(object sender, EventArgs e)
        {
            if (CurrentDate <= DateTime.Today)
                return;

            CurrentDate -= TimeSpan.FromDays(1);
            UpdateDateText();
        }

        private void btnNextDate_Click(object sender, EventArgs e)
        {
            CurrentDate += TimeSpan.FromDays(1);
            UpdateDateText();
        }
    }
}
