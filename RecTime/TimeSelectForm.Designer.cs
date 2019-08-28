namespace RecTime
{
    partial class TimeSelectForm
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
            this.materialRaisedButtonCancel = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialRaisedButton2 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.numericMinutes = new System.Windows.Forms.NumericUpDown();
            this.numericSeconds = new System.Windows.Forms.NumericUpDown();
            this.materialLabelSeconds = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabelMinutes = new MaterialSkin.Controls.MaterialLabel();
            ((System.ComponentModel.ISupportInitialize)(this.numericMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSeconds)).BeginInit();
            this.SuspendLayout();
            // 
            // materialRaisedButtonCancel
            // 
            this.materialRaisedButtonCancel.AutoSize = true;
            this.materialRaisedButtonCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialRaisedButtonCancel.Depth = 0;
            this.materialRaisedButtonCancel.Icon = null;
            this.materialRaisedButtonCancel.Location = new System.Drawing.Point(125, 167);
            this.materialRaisedButtonCancel.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButtonCancel.Name = "materialRaisedButtonCancel";
            this.materialRaisedButtonCancel.Primary = true;
            this.materialRaisedButtonCancel.Size = new System.Drawing.Size(73, 36);
            this.materialRaisedButtonCancel.TabIndex = 0;
            this.materialRaisedButtonCancel.Text = "Avbryt";
            this.materialRaisedButtonCancel.UseVisualStyleBackColor = true;
            this.materialRaisedButtonCancel.Click += new System.EventHandler(this.materialRaisedButtonCancel_Click);
            // 
            // materialRaisedButton2
            // 
            this.materialRaisedButton2.AutoSize = true;
            this.materialRaisedButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialRaisedButton2.Depth = 0;
            this.materialRaisedButton2.Icon = null;
            this.materialRaisedButton2.Location = new System.Drawing.Point(31, 167);
            this.materialRaisedButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton2.Name = "materialRaisedButton2";
            this.materialRaisedButton2.Primary = true;
            this.materialRaisedButton2.Size = new System.Drawing.Size(39, 36);
            this.materialRaisedButton2.TabIndex = 1;
            this.materialRaisedButton2.Text = "OK";
            this.materialRaisedButton2.UseVisualStyleBackColor = true;
            this.materialRaisedButton2.Click += new System.EventHandler(this.materialRaisedButton2_Click);
            // 
            // numericMinutes
            // 
            this.numericMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericMinutes.Location = new System.Drawing.Point(31, 113);
            this.numericMinutes.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericMinutes.Name = "numericMinutes";
            this.numericMinutes.Size = new System.Drawing.Size(73, 24);
            this.numericMinutes.TabIndex = 2;
            this.numericMinutes.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // numericSeconds
            // 
            this.numericSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericSeconds.Location = new System.Drawing.Point(125, 113);
            this.numericSeconds.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericSeconds.Name = "numericSeconds";
            this.numericSeconds.Size = new System.Drawing.Size(73, 24);
            this.numericSeconds.TabIndex = 3;
            // 
            // materialLabelSeconds
            // 
            this.materialLabelSeconds.AutoSize = true;
            this.materialLabelSeconds.Depth = 0;
            this.materialLabelSeconds.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabelSeconds.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabelSeconds.Location = new System.Drawing.Point(121, 91);
            this.materialLabelSeconds.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabelSeconds.Name = "materialLabelSeconds";
            this.materialLabelSeconds.Size = new System.Drawing.Size(75, 19);
            this.materialLabelSeconds.TabIndex = 4;
            this.materialLabelSeconds.Text = "Sekunder:";
            // 
            // materialLabelMinutes
            // 
            this.materialLabelMinutes.AutoSize = true;
            this.materialLabelMinutes.Depth = 0;
            this.materialLabelMinutes.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabelMinutes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabelMinutes.Location = new System.Drawing.Point(27, 91);
            this.materialLabelMinutes.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabelMinutes.Name = "materialLabelMinutes";
            this.materialLabelMinutes.Size = new System.Drawing.Size(64, 19);
            this.materialLabelMinutes.TabIndex = 5;
            this.materialLabelMinutes.Text = "Minuter:";
            // 
            // TimeSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 220);
            this.Controls.Add(this.materialLabelMinutes);
            this.Controls.Add(this.materialLabelSeconds);
            this.Controls.Add(this.numericSeconds);
            this.Controls.Add(this.numericMinutes);
            this.Controls.Add(this.materialRaisedButton2);
            this.Controls.Add(this.materialRaisedButtonCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeSelectForm";
            this.Sizable = false;
            this.Text = "Välj längd för inspelning";
            ((System.ComponentModel.ISupportInitialize)(this.numericMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericSeconds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButtonCancel;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton2;
        private System.Windows.Forms.NumericUpDown numericMinutes;
        private System.Windows.Forms.NumericUpDown numericSeconds;
        private MaterialSkin.Controls.MaterialLabel materialLabelSeconds;
        private MaterialSkin.Controls.MaterialLabel materialLabelMinutes;
    }
}