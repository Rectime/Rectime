namespace RecTime
{
    partial class RecTime
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecTime));
            this.materialTabControl1 = new MaterialSkin.Controls.MaterialTabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnStartDownload = new MaterialSkin.Controls.MaterialRaisedButton();
            this.txtFilename = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.lblInfo = new MaterialSkin.Controls.MaterialLabel();
            this.txtOutputLocation = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.txtUrl = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.lblChannel = new MaterialSkin.Controls.MaterialLabel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.listViewQueue = new MaterialSkin.Controls.MaterialListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.labelFFmpeg = new System.Windows.Forms.Label();
            this.materialLabelVersion = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.labelLength = new MaterialSkin.Controls.MaterialLabel();
            this.numericUpDownDuration = new System.Windows.Forms.NumericUpDown();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.radioBtnKunskap = new MaterialSkin.Controls.MaterialRadioButton();
            this.radioBtnSvt24 = new MaterialSkin.Controls.MaterialRadioButton();
            this.radioBtnSvt2 = new MaterialSkin.Controls.MaterialRadioButton();
            this.radioBtnSvt1 = new MaterialSkin.Controls.MaterialRadioButton();
            this.btnAddLive = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialTabSelector1 = new MaterialSkin.Controls.MaterialTabSelector();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.materialDivider1 = new MaterialSkin.Controls.MaterialDivider();
            this.lblStatus = new MaterialSkin.Controls.MaterialLabel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.btnSvt24 = new System.Windows.Forms.Button();
            this.btnSvtKunskap = new System.Windows.Forms.Button();
            this.btnSvt2 = new System.Windows.Forms.Button();
            this.btnSvt1 = new System.Windows.Forms.Button();
            this.btnChannelSvtb = new System.Windows.Forms.Button();
            this.pictureBoxFFmpeg = new System.Windows.Forms.PictureBox();
            this.pictureBoxDonate = new System.Windows.Forms.PictureBox();
            this.materialTabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFFmpeg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDonate)).BeginInit();
            this.SuspendLayout();
            // 
            // materialTabControl1
            // 
            this.materialTabControl1.Controls.Add(this.tabPage1);
            this.materialTabControl1.Controls.Add(this.tabPage5);
            this.materialTabControl1.Controls.Add(this.tabPage2);
            this.materialTabControl1.Controls.Add(this.tabPage3);
            this.materialTabControl1.Depth = 0;
            this.materialTabControl1.Location = new System.Drawing.Point(12, 125);
            this.materialTabControl1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabControl1.Name = "materialTabControl1";
            this.materialTabControl1.SelectedIndex = 0;
            this.materialTabControl1.Size = new System.Drawing.Size(524, 337);
            this.materialTabControl1.TabIndex = 0;
            this.materialTabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.materialTabControl1_Selected);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.White;
            this.tabPage1.Controls.Add(this.pictureBox);
            this.tabPage1.Controls.Add(this.btnStartDownload);
            this.tabPage1.Controls.Add(this.txtFilename);
            this.tabPage1.Controls.Add(this.lblInfo);
            this.tabPage1.Controls.Add(this.txtOutputLocation);
            this.tabPage1.Controls.Add(this.txtUrl);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(516, 311);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Lägg till";
            // 
            // btnStartDownload
            // 
            this.btnStartDownload.AutoSize = true;
            this.btnStartDownload.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnStartDownload.Depth = 0;
            this.btnStartDownload.Icon = null;
            this.btnStartDownload.Location = new System.Drawing.Point(350, 288);
            this.btnStartDownload.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnStartDownload.Name = "btnStartDownload";
            this.btnStartDownload.Primary = true;
            this.btnStartDownload.Size = new System.Drawing.Size(170, 36);
            this.btnStartDownload.TabIndex = 5;
            this.btnStartDownload.Text = "Starta nedladdning";
            this.btnStartDownload.UseVisualStyleBackColor = true;
            this.btnStartDownload.Visible = false;
            this.btnStartDownload.Click += new System.EventHandler(this.btnStartDownload_Click);
            // 
            // txtFilename
            // 
            this.txtFilename.Depth = 0;
            this.txtFilename.Hint = "Set filename";
            this.txtFilename.Location = new System.Drawing.Point(9, 64);
            this.txtFilename.MaxLength = 32767;
            this.txtFilename.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.PasswordChar = '\0';
            this.txtFilename.SelectedText = "";
            this.txtFilename.SelectionLength = 0;
            this.txtFilename.SelectionStart = 0;
            this.txtFilename.Size = new System.Drawing.Size(507, 23);
            this.txtFilename.TabIndex = 4;
            this.txtFilename.TabStop = false;
            this.txtFilename.Text = "filenamn.mp4";
            this.txtFilename.UseSystemPasswordChar = false;
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Depth = 0;
            this.lblInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblInfo.Location = new System.Drawing.Point(5, 90);
            this.lblInfo.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(78, 18);
            this.lblInfo.TabIndex = 3;
            this.lblInfo.Text = "Välj ström:";
            // 
            // txtOutputLocation
            // 
            this.txtOutputLocation.Depth = 0;
            this.txtOutputLocation.Hint = "Välj mapp";
            this.txtOutputLocation.Location = new System.Drawing.Point(9, 35);
            this.txtOutputLocation.MaxLength = 32767;
            this.txtOutputLocation.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtOutputLocation.Name = "txtOutputLocation";
            this.txtOutputLocation.PasswordChar = '\0';
            this.txtOutputLocation.SelectedText = "";
            this.txtOutputLocation.SelectionLength = 0;
            this.txtOutputLocation.SelectionStart = 0;
            this.txtOutputLocation.Size = new System.Drawing.Size(524, 23);
            this.txtOutputLocation.TabIndex = 2;
            this.txtOutputLocation.TabStop = false;
            this.txtOutputLocation.UseSystemPasswordChar = false;
            this.txtOutputLocation.Enter += new System.EventHandler(this.txtOutputLocation_Enter);
            // 
            // txtUrl
            // 
            this.txtUrl.Depth = 0;
            this.txtUrl.Hint = "Kopiera Url adress";
            this.txtUrl.Location = new System.Drawing.Point(9, 6);
            this.txtUrl.MaxLength = 32767;
            this.txtUrl.MouseState = MaterialSkin.MouseState.HOVER;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.PasswordChar = '\0';
            this.txtUrl.SelectedText = "";
            this.txtUrl.SelectionLength = 0;
            this.txtUrl.SelectionStart = 0;
            this.txtUrl.Size = new System.Drawing.Size(524, 23);
            this.txtUrl.TabIndex = 1;
            this.txtUrl.TabStop = false;
            this.txtUrl.UseSystemPasswordChar = false;
            this.txtUrl.Enter += new System.EventHandler(this.txtUrl_Enter);
            this.txtUrl.TextChanged += new System.EventHandler(this.txtUrl_TextChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.lblChannel);
            this.tabPage5.Controls.Add(this.btnSvt24);
            this.tabPage5.Controls.Add(this.btnSvtKunskap);
            this.tabPage5.Controls.Add(this.btnSvt2);
            this.tabPage5.Controls.Add(this.btnSvt1);
            this.tabPage5.Controls.Add(this.btnChannelSvtb);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(516, 311);
            this.tabPage5.TabIndex = 3;
            this.tabPage5.Text = "Live";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // lblChannel
            // 
            this.lblChannel.AutoSize = true;
            this.lblChannel.Depth = 0;
            this.lblChannel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblChannel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblChannel.Location = new System.Drawing.Point(4, 18);
            this.lblChannel.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblChannel.Name = "lblChannel";
            this.lblChannel.Size = new System.Drawing.Size(70, 18);
            this.lblChannel.TabIndex = 6;
            this.lblChannel.Text = "Välj kanal";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.listViewQueue);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(516, 311);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Kö";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // listViewQueue
            // 
            this.listViewQueue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewQueue.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderType,
            this.columnHeaderStatus});
            this.listViewQueue.Depth = 0;
            this.listViewQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.listViewQueue.FullRowSelect = true;
            this.listViewQueue.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listViewQueue.Location = new System.Drawing.Point(3, 6);
            this.listViewQueue.MouseLocation = new System.Drawing.Point(-1, -1);
            this.listViewQueue.MouseState = MaterialSkin.MouseState.OUT;
            this.listViewQueue.Name = "listViewQueue";
            this.listViewQueue.OwnerDraw = true;
            this.listViewQueue.Size = new System.Drawing.Size(507, 305);
            this.listViewQueue.TabIndex = 0;
            this.listViewQueue.UseCompatibleStateImageBehavior = false;
            this.listViewQueue.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Namn";
            this.columnHeaderName.Width = 252;
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Typ";
            this.columnHeaderType.Width = 85;
            // 
            // columnHeaderStatus
            // 
            this.columnHeaderStatus.Text = "Status";
            this.columnHeaderStatus.Width = 99;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.labelFFmpeg);
            this.tabPage3.Controls.Add(this.materialLabelVersion);
            this.tabPage3.Controls.Add(this.materialLabel1);
            this.tabPage3.Controls.Add(this.pictureBoxFFmpeg);
            this.tabPage3.Controls.Add(this.pictureBoxDonate);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(516, 311);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Om";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // labelFFmpeg
            // 
            this.labelFFmpeg.AutoSize = true;
            this.labelFFmpeg.Location = new System.Drawing.Point(419, 286);
            this.labelFFmpeg.Name = "labelFFmpeg";
            this.labelFFmpeg.Size = new System.Drawing.Size(62, 13);
            this.labelFFmpeg.TabIndex = 4;
            this.labelFFmpeg.Text = "powered by";
            // 
            // materialLabelVersion
            // 
            this.materialLabelVersion.AutoSize = true;
            this.materialLabelVersion.Depth = 0;
            this.materialLabelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.materialLabelVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabelVersion.Location = new System.Drawing.Point(14, 34);
            this.materialLabelVersion.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabelVersion.Name = "materialLabelVersion";
            this.materialLabelVersion.Size = new System.Drawing.Size(0, 18);
            this.materialLabelVersion.TabIndex = 2;
            // 
            // materialLabel1
            // 
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(15, 67);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(489, 186);
            this.materialLabel1.TabIndex = 0;
            this.materialLabel1.Text = resources.GetString("materialLabel1.Text");
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.labelLength);
            this.tabPage4.Controls.Add(this.numericUpDownDuration);
            this.tabPage4.Controls.Add(this.dateTimePickerStart);
            this.tabPage4.Controls.Add(this.radioBtnKunskap);
            this.tabPage4.Controls.Add(this.radioBtnSvt24);
            this.tabPage4.Controls.Add(this.radioBtnSvt2);
            this.tabPage4.Controls.Add(this.radioBtnSvt1);
            this.tabPage4.Controls.Add(this.btnAddLive);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(516, 311);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Live";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Depth = 0;
            this.labelLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.labelLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelLength.Location = new System.Drawing.Point(34, 250);
            this.labelLength.MouseState = MaterialSkin.MouseState.HOVER;
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(115, 18);
            this.labelLength.TabIndex = 7;
            this.labelLength.Text = "Längd (minuter):";
            // 
            // numericUpDownDuration
            // 
            this.numericUpDownDuration.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericUpDownDuration.Location = new System.Drawing.Point(38, 272);
            this.numericUpDownDuration.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDownDuration.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownDuration.Name = "numericUpDownDuration";
            this.numericUpDownDuration.Size = new System.Drawing.Size(120, 23);
            this.numericUpDownDuration.TabIndex = 6;
            this.numericUpDownDuration.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerStart.Location = new System.Drawing.Point(38, 201);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(238, 23);
            this.dateTimePickerStart.TabIndex = 5;
            // 
            // radioBtnKunskap
            // 
            this.radioBtnKunskap.AutoSize = true;
            this.radioBtnKunskap.Depth = 0;
            this.radioBtnKunskap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.radioBtnKunskap.Location = new System.Drawing.Point(38, 123);
            this.radioBtnKunskap.Margin = new System.Windows.Forms.Padding(0);
            this.radioBtnKunskap.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radioBtnKunskap.MouseState = MaterialSkin.MouseState.HOVER;
            this.radioBtnKunskap.Name = "radioBtnKunskap";
            this.radioBtnKunskap.Ripple = true;
            this.radioBtnKunskap.Size = new System.Drawing.Size(138, 30);
            this.radioBtnKunskap.TabIndex = 4;
            this.radioBtnKunskap.TabStop = true;
            this.radioBtnKunskap.Tag = "barnkanalen";
            this.radioBtnKunskap.Text = "Kunskapskanalen";
            this.radioBtnKunskap.UseVisualStyleBackColor = true;
            this.radioBtnKunskap.CheckedChanged += new System.EventHandler(this.radioBtnLive_CheckedChanged);
            // 
            // radioBtnSvt24
            // 
            this.radioBtnSvt24.AutoSize = true;
            this.radioBtnSvt24.Depth = 0;
            this.radioBtnSvt24.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.radioBtnSvt24.Location = new System.Drawing.Point(38, 93);
            this.radioBtnSvt24.Margin = new System.Windows.Forms.Padding(0);
            this.radioBtnSvt24.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radioBtnSvt24.MouseState = MaterialSkin.MouseState.HOVER;
            this.radioBtnSvt24.Name = "radioBtnSvt24";
            this.radioBtnSvt24.Ripple = true;
            this.radioBtnSvt24.Size = new System.Drawing.Size(159, 30);
            this.radioBtnSvt24.TabIndex = 3;
            this.radioBtnSvt24.TabStop = true;
            this.radioBtnSvt24.Tag = "svt24";
            this.radioBtnSvt24.Text = "SVT24 / Barnkanalen";
            this.radioBtnSvt24.UseVisualStyleBackColor = true;
            this.radioBtnSvt24.CheckedChanged += new System.EventHandler(this.radioBtnLive_CheckedChanged);
            // 
            // radioBtnSvt2
            // 
            this.radioBtnSvt2.AutoSize = true;
            this.radioBtnSvt2.Depth = 0;
            this.radioBtnSvt2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.radioBtnSvt2.Location = new System.Drawing.Point(38, 63);
            this.radioBtnSvt2.Margin = new System.Windows.Forms.Padding(0);
            this.radioBtnSvt2.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radioBtnSvt2.MouseState = MaterialSkin.MouseState.HOVER;
            this.radioBtnSvt2.Name = "radioBtnSvt2";
            this.radioBtnSvt2.Ripple = true;
            this.radioBtnSvt2.Size = new System.Drawing.Size(66, 30);
            this.radioBtnSvt2.TabIndex = 2;
            this.radioBtnSvt2.TabStop = true;
            this.radioBtnSvt2.Tag = "svt2";
            this.radioBtnSvt2.Text = "SVT 2";
            this.radioBtnSvt2.UseVisualStyleBackColor = true;
            this.radioBtnSvt2.CheckedChanged += new System.EventHandler(this.radioBtnLive_CheckedChanged);
            // 
            // radioBtnSvt1
            // 
            this.radioBtnSvt1.AutoSize = true;
            this.radioBtnSvt1.Depth = 0;
            this.radioBtnSvt1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.radioBtnSvt1.Location = new System.Drawing.Point(38, 33);
            this.radioBtnSvt1.Margin = new System.Windows.Forms.Padding(0);
            this.radioBtnSvt1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.radioBtnSvt1.MouseState = MaterialSkin.MouseState.HOVER;
            this.radioBtnSvt1.Name = "radioBtnSvt1";
            this.radioBtnSvt1.Ripple = true;
            this.radioBtnSvt1.Size = new System.Drawing.Size(66, 30);
            this.radioBtnSvt1.TabIndex = 1;
            this.radioBtnSvt1.TabStop = true;
            this.radioBtnSvt1.Tag = "svt1";
            this.radioBtnSvt1.Text = "SVT 1";
            this.radioBtnSvt1.UseVisualStyleBackColor = true;
            this.radioBtnSvt1.CheckedChanged += new System.EventHandler(this.radioBtnLive_CheckedChanged);
            // 
            // btnAddLive
            // 
            this.btnAddLive.AutoSize = true;
            this.btnAddLive.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddLive.Depth = 0;
            this.btnAddLive.Icon = null;
            this.btnAddLive.Location = new System.Drawing.Point(389, 272);
            this.btnAddLive.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddLive.Name = "btnAddLive";
            this.btnAddLive.Primary = true;
            this.btnAddLive.Size = new System.Drawing.Size(124, 36);
            this.btnAddLive.TabIndex = 0;
            this.btnAddLive.Text = "Start Live Rec";
            this.btnAddLive.UseVisualStyleBackColor = true;
            // 
            // materialTabSelector1
            // 
            this.materialTabSelector1.BaseTabControl = this.materialTabControl1;
            this.materialTabSelector1.Depth = 0;
            this.materialTabSelector1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.materialTabSelector1.Location = new System.Drawing.Point(-1, 63);
            this.materialTabSelector1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialTabSelector1.Name = "materialTabSelector1";
            this.materialTabSelector1.Size = new System.Drawing.Size(554, 56);
            this.materialTabSelector1.TabIndex = 1;
            this.materialTabSelector1.Text = "materialTabSelector1";
            // 
            // materialDivider1
            // 
            this.materialDivider1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(31)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialDivider1.Depth = 0;
            this.materialDivider1.Location = new System.Drawing.Point(0, 461);
            this.materialDivider1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialDivider1.Name = "materialDivider1";
            this.materialDivider1.Size = new System.Drawing.Size(553, 1);
            this.materialDivider1.TabIndex = 2;
            this.materialDivider1.Text = "materialDivider1";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Depth = 0;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblStatus.Location = new System.Drawing.Point(21, 473);
            this.lblStatus.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(54, 18);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Status:";
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLogo.Image = global::RecTime.Properties.Resources.icon_small;
            this.pictureBoxLogo.Location = new System.Drawing.Point(6, 25);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(34, 36);
            this.pictureBoxLogo.TabIndex = 7;
            this.pictureBoxLogo.TabStop = false;
            // 
            // pictureBox
            // 
            this.pictureBox.Location = new System.Drawing.Point(274, 115);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(242, 148);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 6;
            this.pictureBox.TabStop = false;
            // 
            // btnSvt24
            // 
            this.btnSvt24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.btnSvt24.Image = global::RecTime.Properties.Resources.svt24;
            this.btnSvt24.Location = new System.Drawing.Point(241, 100);
            this.btnSvt24.Name = "btnSvt24";
            this.btnSvt24.Size = new System.Drawing.Size(227, 54);
            this.btnSvt24.TabIndex = 5;
            this.btnSvt24.Tag = "Svt24";
            this.btnSvt24.UseVisualStyleBackColor = false;
            this.btnSvt24.Click += new System.EventHandler(this.btnChannel_Click);
            // 
            // btnSvtKunskap
            // 
            this.btnSvtKunskap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.btnSvtKunskap.Image = global::RecTime.Properties.Resources.kunskapskanalen;
            this.btnSvtKunskap.Location = new System.Drawing.Point(241, 40);
            this.btnSvtKunskap.Name = "btnSvtKunskap";
            this.btnSvtKunskap.Size = new System.Drawing.Size(227, 54);
            this.btnSvtKunskap.TabIndex = 4;
            this.btnSvtKunskap.Tag = "Kunskapskanalen";
            this.btnSvtKunskap.UseVisualStyleBackColor = false;
            this.btnSvtKunskap.Click += new System.EventHandler(this.btnChannel_Click);
            // 
            // btnSvt2
            // 
            this.btnSvt2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.btnSvt2.Image = global::RecTime.Properties.Resources.svt2;
            this.btnSvt2.Location = new System.Drawing.Point(8, 160);
            this.btnSvt2.Name = "btnSvt2";
            this.btnSvt2.Size = new System.Drawing.Size(227, 54);
            this.btnSvt2.TabIndex = 3;
            this.btnSvt2.Tag = "Svt2";
            this.btnSvt2.UseVisualStyleBackColor = false;
            this.btnSvt2.Click += new System.EventHandler(this.btnChannel_Click);
            // 
            // btnSvt1
            // 
            this.btnSvt1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.btnSvt1.Image = global::RecTime.Properties.Resources.svt1;
            this.btnSvt1.Location = new System.Drawing.Point(8, 100);
            this.btnSvt1.Name = "btnSvt1";
            this.btnSvt1.Size = new System.Drawing.Size(227, 54);
            this.btnSvt1.TabIndex = 2;
            this.btnSvt1.Tag = "Svt1";
            this.btnSvt1.UseVisualStyleBackColor = false;
            this.btnSvt1.Click += new System.EventHandler(this.btnChannel_Click);
            // 
            // btnChannelSvtb
            // 
            this.btnChannelSvtb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.btnChannelSvtb.Image = global::RecTime.Properties.Resources.barnkanalen;
            this.btnChannelSvtb.Location = new System.Drawing.Point(8, 40);
            this.btnChannelSvtb.Name = "btnChannelSvtb";
            this.btnChannelSvtb.Size = new System.Drawing.Size(227, 54);
            this.btnChannelSvtb.TabIndex = 1;
            this.btnChannelSvtb.Tag = "Barnkanalen";
            this.btnChannelSvtb.UseVisualStyleBackColor = false;
            this.btnChannelSvtb.Click += new System.EventHandler(this.btnChannel_Click);
            // 
            // pictureBoxFFmpeg
            // 
            this.pictureBoxFFmpeg.Image = global::RecTime.Properties.Resources.ffmpeg;
            this.pictureBoxFFmpeg.Location = new System.Drawing.Point(383, 294);
            this.pictureBoxFFmpeg.Name = "pictureBoxFFmpeg";
            this.pictureBoxFFmpeg.Size = new System.Drawing.Size(133, 35);
            this.pictureBoxFFmpeg.TabIndex = 3;
            this.pictureBoxFFmpeg.TabStop = false;
            this.pictureBoxFFmpeg.Click += new System.EventHandler(this.pictureBoxFFmpeg_Click);
            // 
            // pictureBoxDonate
            // 
            this.pictureBoxDonate.Image = global::RecTime.Properties.Resources.donate;
            this.pictureBoxDonate.Location = new System.Drawing.Point(206, 280);
            this.pictureBoxDonate.Name = "pictureBoxDonate";
            this.pictureBoxDonate.Size = new System.Drawing.Size(109, 57);
            this.pictureBoxDonate.TabIndex = 1;
            this.pictureBoxDonate.TabStop = false;
            this.pictureBoxDonate.Click += new System.EventHandler(this.pictureBoxDonate_Click);
            // 
            // RecTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(548, 501);
            this.Controls.Add(this.pictureBoxLogo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.materialDivider1);
            this.Controls.Add(this.materialTabSelector1);
            this.Controls.Add(this.materialTabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RecTime";
            this.Sizable = false;
            this.Text = "      RecTime";
            this.Activated += new System.EventHandler(this.RecTime_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RecTime_FormClosing);
            this.Load += new System.EventHandler(this.RecTime_Load);
            this.materialTabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownDuration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFFmpeg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDonate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialTabControl materialTabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private MaterialSkin.Controls.MaterialTabSelector materialTabSelector1;
        private System.Windows.Forms.TabPage tabPage3;
        private MaterialSkin.Controls.MaterialSingleLineTextField txtUrl;
        private MaterialSkin.Controls.MaterialSingleLineTextField txtOutputLocation;
        private MaterialSkin.Controls.MaterialLabel lblInfo;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private MaterialSkin.Controls.MaterialDivider materialDivider1;
        private MaterialSkin.Controls.MaterialSingleLineTextField txtFilename;
        private MaterialSkin.Controls.MaterialRaisedButton btnStartDownload;
        private System.Windows.Forms.PictureBox pictureBox;
        private MaterialSkin.Controls.MaterialLabel lblStatus;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialListView listViewQueue;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderStatus;
        private System.Windows.Forms.TabPage tabPage4;
        private MaterialSkin.Controls.MaterialRaisedButton btnAddLive;
        private MaterialSkin.Controls.MaterialRadioButton radioBtnSvt1;
        private MaterialSkin.Controls.MaterialRadioButton radioBtnSvt2;
        private MaterialSkin.Controls.MaterialRadioButton radioBtnKunskap;
        private MaterialSkin.Controls.MaterialRadioButton radioBtnSvt24;
        private System.Windows.Forms.NumericUpDown numericUpDownDuration;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private MaterialSkin.Controls.MaterialLabel labelLength;
        private System.Windows.Forms.PictureBox pictureBoxDonate;
        private MaterialSkin.Controls.MaterialLabel materialLabelVersion;
        private System.Windows.Forms.Label labelFFmpeg;
        private System.Windows.Forms.PictureBox pictureBoxFFmpeg;
        private System.Windows.Forms.TabPage tabPage5;
        private MaterialSkin.Controls.MaterialLabel lblChannel;
        private System.Windows.Forms.Button btnSvt24;
        private System.Windows.Forms.Button btnSvtKunskap;
        private System.Windows.Forms.Button btnSvt2;
        private System.Windows.Forms.Button btnSvt1;
        private System.Windows.Forms.Button btnChannelSvtb;
    }
}

