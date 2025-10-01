namespace NT.IPTV
{
    partial class frmDownloader
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownloader));
            lstLog = new ListBox();
            picMovie = new PictureBox();
            MyToolTip = new ToolTip(components);
            lblFileName = new Label();
            statusStrip1 = new StatusStrip();
            lblPercentage = new ToolStripStatusLabel();
            prgBar = new ToolStripProgressBar();
            lblOverallProgress = new ToolStripStatusLabel();
            prgBarSeries = new ToolStripProgressBar();
            ((System.ComponentModel.ISupportInitialize)picMovie).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lstLog
            // 
            lstLog.BackColor = Color.FromArgb(64, 64, 64);
            lstLog.Dock = DockStyle.Right;
            lstLog.Font = new Font("Segoe UI", 9F);
            lstLog.ForeColor = Color.Black;
            lstLog.Location = new Point(625, 0);
            lstLog.Margin = new Padding(0);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(721, 339);
            lstLog.TabIndex = 5;
            // 
            // picMovie
            // 
            picMovie.BackColor = Color.FromArgb(64, 64, 64);
            picMovie.Cursor = Cursors.Hand;
            picMovie.Dock = DockStyle.Fill;
            picMovie.Location = new Point(0, 0);
            picMovie.Margin = new Padding(4);
            picMovie.Name = "picMovie";
            picMovie.Size = new Size(625, 339);
            picMovie.SizeMode = PictureBoxSizeMode.Zoom;
            picMovie.TabIndex = 7;
            picMovie.TabStop = false;
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Dock = DockStyle.Bottom;
            lblFileName.Location = new Point(0, 339);
            lblFileName.Margin = new Padding(4, 0, 4, 0);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(91, 25);
            lblFileName.TabIndex = 10;
            lblFileName.Text = "FileName";
            lblFileName.Click += lblFileName_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblPercentage, prgBar, lblOverallProgress, prgBarSeries });
            statusStrip1.Location = new Point(0, 364);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 18, 0);
            statusStrip1.Size = new Size(1346, 34);
            statusStrip1.TabIndex = 11;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblPercentage
            // 
            lblPercentage.AutoSize = false;
            lblPercentage.Name = "lblPercentage";
            lblPercentage.Size = new Size(250, 28);
            lblPercentage.Text = "Percentage";
            lblPercentage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // prgBar
            // 
            prgBar.Name = "prgBar";
            prgBar.Size = new Size(250, 26);
            // 
            // lblOverallProgress
            // 
            lblOverallProgress.Name = "lblOverallProgress";
            lblOverallProgress.Size = new Size(116, 28);
            lblOverallProgress.Text = "Overall Progress";
            // 
            // prgBarSeries
            // 
            prgBarSeries.Name = "prgBarSeries";
            prgBarSeries.Size = new Size(250, 26);
            // 
            // frmDownloader
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(1346, 398);
            Controls.Add(picMovie);
            Controls.Add(lstLog);
            Controls.Add(lblFileName);
            Controls.Add(statusStrip1);
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 11F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDownloader";
            Text = "Downloader";
            FormClosing += frmDownloader_FormClosing;
            Load += frmDownloader_Load;
            ((System.ComponentModel.ISupportInitialize)picMovie).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ListBox lstLog;
        private PictureBox picMovie;
        private ToolTip MyToolTip;
        private Label lblFileName;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblPercentage;
        private ToolStripProgressBar prgBar;
        private ToolStripStatusLabel lblOverallProgress;
        private ToolStripProgressBar prgBarSeries;
    }
}
