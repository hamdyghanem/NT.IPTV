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
            lblInfo = new Label();
            picMovie = new PictureBox();
            MyToolTip = new ToolTip(components);
            lblFileName = new Label();
            statusStrip1 = new StatusStrip();
            lblPercentage = new ToolStripStatusLabel();
            prgBar = new ToolStripProgressBar();
            lblLinks = new ToolStripStatusLabel();
            lblOverallProgress = new ToolStripStatusLabel();
            prgBarSeries = new ToolStripProgressBar();
            ((System.ComponentModel.ISupportInitialize)picMovie).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // lblInfo
            // 
            lblInfo.BackColor = Color.FromArgb(64, 64, 64);
            lblInfo.Dock = DockStyle.Right;
            lblInfo.Font = new Font("Segoe UI", 9F);
            lblInfo.ForeColor = Color.Black;
            lblInfo.Location = new Point(655, 0);
            lblInfo.Margin = new Padding(0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(422, 272);
            lblInfo.TabIndex = 5;
            lblInfo.Text = "label1";
            lblInfo.TextAlign = ContentAlignment.TopRight;
            // 
            // picMovie
            // 
            picMovie.BackColor = Color.FromArgb(64, 64, 64);
            picMovie.Cursor = Cursors.Hand;
            picMovie.Dock = DockStyle.Fill;
            picMovie.Location = new Point(0, 0);
            picMovie.Name = "picMovie";
            picMovie.Size = new Size(655, 272);
            picMovie.SizeMode = PictureBoxSizeMode.Zoom;
            picMovie.TabIndex = 7;
            picMovie.TabStop = false;
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Dock = DockStyle.Bottom;
            lblFileName.Location = new Point(0, 272);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(72, 20);
            lblFileName.TabIndex = 10;
            lblFileName.Text = "FileName";
            lblFileName.Click += lblFileName_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblPercentage, prgBar, lblLinks, lblOverallProgress, prgBarSeries });
            statusStrip1.Location = new Point(0, 292);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1077, 26);
            statusStrip1.TabIndex = 11;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblPercentage
            // 
            lblPercentage.AutoSize = false;
            lblPercentage.Name = "lblPercentage";
            lblPercentage.Size = new Size(200, 20);
            lblPercentage.Text = "Percentage";
            // 
            // prgBar
            // 
            prgBar.Name = "prgBar";
            prgBar.Size = new Size(200, 18);
            // 
            // lblLinks
            // 
            lblLinks.Name = "lblLinks";
            lblLinks.Size = new Size(79, 20);
            lblLinks.Text = "Copy Links";
            lblLinks.Click += lblLinks_Click;
            // 
            // lblOverallProgress
            // 
            lblOverallProgress.Name = "lblOverallProgress";
            lblOverallProgress.Size = new Size(116, 20);
            lblOverallProgress.Text = "Overall Progress";
            // 
            // prgBarSeries
            // 
            prgBarSeries.Name = "prgBarSeries";
            prgBarSeries.Size = new Size(200, 18);
            // 
            // frmDownloader
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            ClientSize = new Size(1077, 318);
            Controls.Add(picMovie);
            Controls.Add(lblInfo);
            Controls.Add(lblFileName);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
        private Label lblInfo;
        private PictureBox picMovie;
        private ToolTip MyToolTip;
        private Label lblFileName;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblPercentage;
        private ToolStripProgressBar prgBar;
        private ToolStripStatusLabel lblLinks;
        private ToolStripStatusLabel lblOverallProgress;
        private ToolStripProgressBar prgBarSeries;
    }
}
