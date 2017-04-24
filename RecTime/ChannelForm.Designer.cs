namespace RecTime
{
    partial class ChannelForm
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
            this.btnPrevDate = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnNextDate = new MaterialSkin.Controls.MaterialRaisedButton();
            this.lblDate = new MaterialSkin.Controls.MaterialLabel();
            this.panelPrograms = new System.Windows.Forms.Panel();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.btnAddSelection = new MaterialSkin.Controls.MaterialRaisedButton();
            this.lblDescription = new MaterialSkin.Controls.MaterialLabel();
            this.lblCount = new MaterialSkin.Controls.MaterialLabel();
            this.pictureBoxChannel = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChannel)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrevDate
            // 
            this.btnPrevDate.AutoSize = true;
            this.btnPrevDate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnPrevDate.Depth = 0;
            this.btnPrevDate.Icon = null;
            this.btnPrevDate.Location = new System.Drawing.Point(12, 75);
            this.btnPrevDate.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnPrevDate.Name = "btnPrevDate";
            this.btnPrevDate.Primary = true;
            this.btnPrevDate.Size = new System.Drawing.Size(28, 36);
            this.btnPrevDate.TabIndex = 0;
            this.btnPrevDate.Text = "<";
            this.btnPrevDate.UseVisualStyleBackColor = true;
            this.btnPrevDate.Click += new System.EventHandler(this.btnPrevDate_Click);
            // 
            // btnNextDate
            // 
            this.btnNextDate.AutoSize = true;
            this.btnNextDate.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnNextDate.Depth = 0;
            this.btnNextDate.Icon = null;
            this.btnNextDate.Location = new System.Drawing.Point(147, 75);
            this.btnNextDate.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnNextDate.Name = "btnNextDate";
            this.btnNextDate.Primary = true;
            this.btnNextDate.Size = new System.Drawing.Size(28, 36);
            this.btnNextDate.TabIndex = 1;
            this.btnNextDate.Text = ">";
            this.btnNextDate.UseVisualStyleBackColor = true;
            this.btnNextDate.Click += new System.EventHandler(this.btnNextDate_Click);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Depth = 0;
            this.lblDate.Font = new System.Drawing.Font("Roboto", 11F);
            this.lblDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblDate.Location = new System.Drawing.Point(46, 83);
            this.lblDate.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(37, 19);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Idag";
            // 
            // panelPrograms
            // 
            this.panelPrograms.AutoScroll = true;
            this.panelPrograms.Location = new System.Drawing.Point(12, 117);
            this.panelPrograms.Name = "panelPrograms";
            this.panelPrograms.Size = new System.Drawing.Size(402, 374);
            this.panelPrograms.TabIndex = 3;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLoading.Image = global::RecTime.Properties.Resources.loader;
            this.pictureBoxLoading.Location = new System.Drawing.Point(641, 68);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(33, 34);
            this.pictureBoxLoading.TabIndex = 4;
            this.pictureBoxLoading.TabStop = false;
            // 
            // btnAddSelection
            // 
            this.btnAddSelection.AutoSize = true;
            this.btnAddSelection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnAddSelection.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddSelection.Depth = 0;
            this.btnAddSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSelection.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAddSelection.Icon = null;
            this.btnAddSelection.Location = new System.Drawing.Point(587, 455);
            this.btnAddSelection.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnAddSelection.Name = "btnAddSelection";
            this.btnAddSelection.Primary = true;
            this.btnAddSelection.Size = new System.Drawing.Size(87, 36);
            this.btnAddSelection.TabIndex = 5;
            this.btnAddSelection.Text = "Lägg till";
            this.btnAddSelection.UseVisualStyleBackColor = false;
            this.btnAddSelection.Click += new System.EventHandler(this.btnAddSelection_Click);
            // 
            // lblDescription
            // 
            this.lblDescription.Depth = 0;
            this.lblDescription.Font = new System.Drawing.Font("Roboto", 11F);
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblDescription.Location = new System.Drawing.Point(420, 117);
            this.lblDescription.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(254, 335);
            this.lblDescription.TabIndex = 6;
            // 
            // lblCount
            // 
            this.lblCount.AutoSize = true;
            this.lblCount.Depth = 0;
            this.lblCount.Font = new System.Drawing.Font("Roboto", 11F);
            this.lblCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblCount.Location = new System.Drawing.Point(420, 463);
            this.lblCount.MouseState = MaterialSkin.MouseState.HOVER;
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(0, 19);
            this.lblCount.TabIndex = 7;
            // 
            // pictureBoxChannel
            // 
            this.pictureBoxChannel.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxChannel.Location = new System.Drawing.Point(424, 28);
            this.pictureBoxChannel.Name = "pictureBoxChannel";
            this.pictureBoxChannel.Size = new System.Drawing.Size(211, 36);
            this.pictureBoxChannel.TabIndex = 8;
            this.pictureBoxChannel.TabStop = false;
            // 
            // ChannelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 503);
            this.Controls.Add(this.pictureBoxChannel);
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnAddSelection);
            this.Controls.Add(this.pictureBoxLoading);
            this.Controls.Add(this.panelPrograms);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.btnNextDate);
            this.Controls.Add(this.btnPrevDate);
            this.MaximizeBox = false;
            this.Name = "ChannelForm";
            this.Sizable = false;
            this.Text = "TV-Tablå - ";
            this.Load += new System.EventHandler(this.ChannelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChannel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialRaisedButton btnPrevDate;
        private MaterialSkin.Controls.MaterialRaisedButton btnNextDate;
        private MaterialSkin.Controls.MaterialLabel lblDate;
        private System.Windows.Forms.Panel panelPrograms;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private MaterialSkin.Controls.MaterialRaisedButton btnAddSelection;
        private MaterialSkin.Controls.MaterialLabel lblDescription;
        private MaterialSkin.Controls.MaterialLabel lblCount;
        private System.Windows.Forms.PictureBox pictureBoxChannel;
    }
}