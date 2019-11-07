namespace SuperSight
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBoxCamera = new System.Windows.Forms.GroupBox();
            this.pictureBoxMain = new System.Windows.Forms.PictureBox();
            this.groupBoxControl = new System.Windows.Forms.GroupBox();
            this.textBoxVideoPath = new System.Windows.Forms.TextBox();
            this.buttonBrowse = new DarkUI.Controls.DarkButton();
            this.checkBoxUseWebcam = new System.Windows.Forms.CheckBox();
            this.darkLabel1 = new DarkUI.Controls.DarkLabel();
            this.labelFrameSendRate = new DarkUI.Controls.DarkLabel();
            this.trackBarFrameSendRate = new System.Windows.Forms.TrackBar();
            this.labelFramerate = new DarkUI.Controls.DarkLabel();
            this.labelStatus = new DarkUI.Controls.DarkLabel();
            this.buttonStop = new DarkUI.Controls.DarkButton();
            this.buttonStart = new DarkUI.Controls.DarkButton();
            this.groupBoxStatus = new System.Windows.Forms.GroupBox();
            this.textBoxLog = new DarkUI.Controls.DarkTextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBoxCamera.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).BeginInit();
            this.groupBoxControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrameSendRate)).BeginInit();
            this.groupBoxStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCamera
            // 
            this.groupBoxCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCamera.Controls.Add(this.pictureBoxMain);
            this.groupBoxCamera.Location = new System.Drawing.Point(10, 99);
            this.groupBoxCamera.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxCamera.Name = "groupBoxCamera";
            this.groupBoxCamera.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxCamera.Size = new System.Drawing.Size(1201, 487);
            this.groupBoxCamera.TabIndex = 0;
            this.groupBoxCamera.TabStop = false;
            // 
            // pictureBoxMain
            // 
            this.pictureBoxMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pictureBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxMain.Location = new System.Drawing.Point(2, 15);
            this.pictureBoxMain.Name = "pictureBoxMain";
            this.pictureBoxMain.Size = new System.Drawing.Size(1197, 470);
            this.pictureBoxMain.TabIndex = 0;
            this.pictureBoxMain.TabStop = false;
            // 
            // groupBoxControl
            // 
            this.groupBoxControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxControl.Controls.Add(this.textBoxVideoPath);
            this.groupBoxControl.Controls.Add(this.buttonBrowse);
            this.groupBoxControl.Controls.Add(this.checkBoxUseWebcam);
            this.groupBoxControl.Controls.Add(this.darkLabel1);
            this.groupBoxControl.Controls.Add(this.labelFrameSendRate);
            this.groupBoxControl.Controls.Add(this.trackBarFrameSendRate);
            this.groupBoxControl.Controls.Add(this.labelFramerate);
            this.groupBoxControl.Controls.Add(this.labelStatus);
            this.groupBoxControl.Controls.Add(this.buttonStop);
            this.groupBoxControl.Controls.Add(this.buttonStart);
            this.groupBoxControl.Location = new System.Drawing.Point(9, 10);
            this.groupBoxControl.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxControl.Name = "groupBoxControl";
            this.groupBoxControl.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.groupBoxControl.Size = new System.Drawing.Size(1202, 85);
            this.groupBoxControl.TabIndex = 1;
            this.groupBoxControl.TabStop = false;
            // 
            // textBoxVideoPath
            // 
            this.textBoxVideoPath.Enabled = false;
            this.textBoxVideoPath.Location = new System.Drawing.Point(628, 55);
            this.textBoxVideoPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.textBoxVideoPath.Name = "textBoxVideoPath";
            this.textBoxVideoPath.Size = new System.Drawing.Size(135, 20);
            this.textBoxVideoPath.TabIndex = 12;
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Enabled = false;
            this.buttonBrowse.Location = new System.Drawing.Point(767, 53);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.buttonBrowse.Size = new System.Drawing.Size(39, 23);
            this.buttonBrowse.TabIndex = 11;
            this.buttonBrowse.Text = "...";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // checkBoxUseWebcam
            // 
            this.checkBoxUseWebcam.AutoSize = true;
            this.checkBoxUseWebcam.Checked = true;
            this.checkBoxUseWebcam.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxUseWebcam.ForeColor = System.Drawing.Color.Gainsboro;
            this.checkBoxUseWebcam.Location = new System.Drawing.Point(628, 15);
            this.checkBoxUseWebcam.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.checkBoxUseWebcam.Name = "checkBoxUseWebcam";
            this.checkBoxUseWebcam.Size = new System.Drawing.Size(88, 17);
            this.checkBoxUseWebcam.TabIndex = 10;
            this.checkBoxUseWebcam.Text = "Use webcam";
            this.checkBoxUseWebcam.UseVisualStyleBackColor = true;
            this.checkBoxUseWebcam.CheckedChanged += new System.EventHandler(this.checkBoxUseWebcam_CheckedChanged);
            // 
            // darkLabel1
            // 
            this.darkLabel1.AutoSize = true;
            this.darkLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.darkLabel1.Location = new System.Drawing.Point(625, 36);
            this.darkLabel1.Name = "darkLabel1";
            this.darkLabel1.Size = new System.Drawing.Size(53, 13);
            this.darkLabel1.TabIndex = 9;
            this.darkLabel1.Text = "Video file:";
            // 
            // labelFrameSendRate
            // 
            this.labelFrameSendRate.AutoSize = true;
            this.labelFrameSendRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelFrameSendRate.Location = new System.Drawing.Point(191, 18);
            this.labelFrameSendRate.Name = "labelFrameSendRate";
            this.labelFrameSendRate.Size = new System.Drawing.Size(95, 13);
            this.labelFrameSendRate.TabIndex = 7;
            this.labelFrameSendRate.Text = "Frame send rate: 0";
            // 
            // trackBarFrameSendRate
            // 
            this.trackBarFrameSendRate.LargeChange = 100;
            this.trackBarFrameSendRate.Location = new System.Drawing.Point(194, 35);
            this.trackBarFrameSendRate.Maximum = 1000;
            this.trackBarFrameSendRate.Name = "trackBarFrameSendRate";
            this.trackBarFrameSendRate.Size = new System.Drawing.Size(204, 45);
            this.trackBarFrameSendRate.SmallChange = 20;
            this.trackBarFrameSendRate.TabIndex = 6;
            this.trackBarFrameSendRate.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarFrameSendRate.Value = 100;
            this.trackBarFrameSendRate.Scroll += new System.EventHandler(this.trackBarFrameSendRate_Scroll);
            // 
            // labelFramerate
            // 
            this.labelFramerate.AutoSize = true;
            this.labelFramerate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelFramerate.Location = new System.Drawing.Point(407, 42);
            this.labelFramerate.Name = "labelFramerate";
            this.labelFramerate.Size = new System.Drawing.Size(83, 13);
            this.labelFramerate.TabIndex = 5;
            this.labelFramerate.Text = "Framerate: 0 fps";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.labelStatus.Location = new System.Drawing.Point(407, 18);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(60, 13);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "Status: Idle";
            // 
            // buttonStop
            // 
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(99, 18);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 3;
            this.buttonStop.Text = "Stop";
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(18, 18);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Start";
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupBoxStatus
            // 
            this.groupBoxStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxStatus.Controls.Add(this.textBoxLog);
            this.groupBoxStatus.Location = new System.Drawing.Point(12, 591);
            this.groupBoxStatus.Name = "groupBoxStatus";
            this.groupBoxStatus.Size = new System.Drawing.Size(1196, 162);
            this.groupBoxStatus.TabIndex = 2;
            this.groupBoxStatus.TabStop = false;
            // 
            // textBoxLog
            // 
            this.textBoxLog.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(73)))), ((int)(((byte)(74)))));
            this.textBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLog.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.textBoxLog.Location = new System.Drawing.Point(15, 19);
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLog.Size = new System.Drawing.Size(1164, 126);
            this.textBoxLog.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialogVideo";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1220, 765);
            this.Controls.Add(this.groupBoxStatus);
            this.Controls.Add(this.groupBoxControl);
            this.Controls.Add(this.groupBoxCamera);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FormMain";
            this.Text = "SuperSight";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBoxCamera.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).EndInit();
            this.groupBoxControl.ResumeLayout(false);
            this.groupBoxControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrameSendRate)).EndInit();
            this.groupBoxStatus.ResumeLayout(false);
            this.groupBoxStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxCamera;
        private System.Windows.Forms.GroupBox groupBoxControl;
        private System.Windows.Forms.PictureBox pictureBoxMain;
        private System.Windows.Forms.GroupBox groupBoxStatus;
        private DarkUI.Controls.DarkLabel labelStatus;
        private DarkUI.Controls.DarkButton buttonStop;
        private DarkUI.Controls.DarkButton buttonStart;
        private DarkUI.Controls.DarkTextBox textBoxLog;
        private DarkUI.Controls.DarkLabel labelFramerate;
        private System.Windows.Forms.TrackBar trackBarFrameSendRate;
        private DarkUI.Controls.DarkLabel labelFrameSendRate;
        private DarkUI.Controls.DarkButton buttonBrowse;
        private System.Windows.Forms.CheckBox checkBoxUseWebcam;
        private DarkUI.Controls.DarkLabel darkLabel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBoxVideoPath;
    }
}

