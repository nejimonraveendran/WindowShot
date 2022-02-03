namespace WindowShot
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnStart = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnStop = new System.Windows.Forms.Button();
            this.cmbApplication = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkIsTimeLapse = new System.Windows.Forms.CheckBox();
            this.chkCaptureTitlebar = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(57, 183);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(145, 81);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(223, 183);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(145, 81);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // cmbApplication
            // 
            this.cmbApplication.FormattingEnabled = true;
            this.cmbApplication.Items.AddRange(new object[] {
            "Rebelle 5",
            "Rebelle 4",
            "Rebelle 3",
            "ArtRage",
            "devenv"});
            this.cmbApplication.Location = new System.Drawing.Point(62, 59);
            this.cmbApplication.Name = "cmbApplication";
            this.cmbApplication.Size = new System.Drawing.Size(340, 32);
            this.cmbApplication.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Application";
            // 
            // chkIsTimeLapse
            // 
            this.chkIsTimeLapse.AutoSize = true;
            this.chkIsTimeLapse.Checked = true;
            this.chkIsTimeLapse.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsTimeLapse.Location = new System.Drawing.Point(71, 123);
            this.chkIsTimeLapse.Name = "chkIsTimeLapse";
            this.chkIsTimeLapse.Size = new System.Drawing.Size(129, 29);
            this.chkIsTimeLapse.TabIndex = 9;
            this.chkIsTimeLapse.Text = "Timelapse";
            this.chkIsTimeLapse.UseVisualStyleBackColor = true;
            // 
            // chkCaptureTitlebar
            // 
            this.chkCaptureTitlebar.AutoSize = true;
            this.chkCaptureTitlebar.Location = new System.Drawing.Point(234, 123);
            this.chkCaptureTitlebar.Name = "chkCaptureTitlebar";
            this.chkCaptureTitlebar.Size = new System.Drawing.Size(178, 29);
            this.chkCaptureTitlebar.TabIndex = 10;
            this.chkCaptureTitlebar.Text = "Capture Titlebar";
            this.chkCaptureTitlebar.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(455, 294);
            this.Controls.Add(this.chkCaptureTitlebar);
            this.Controls.Add(this.chkIsTimeLapse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbApplication);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WindowShot";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ComboBox cmbApplication;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkIsTimeLapse;
        private System.Windows.Forms.CheckBox chkCaptureTitlebar;
    }
}

