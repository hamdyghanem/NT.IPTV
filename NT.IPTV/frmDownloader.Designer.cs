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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownloader));
            lblInfo = new Label();
            picMovie = new PictureBox();
            panel1 = new Panel();
            statusStrip = new StatusStrip();
            lblPercentage = new ToolStripStatusLabel();
            lblSize = new ToolStripStatusLabel();
            prgBar = new ToolStripProgressBar();
            lblFileName = new ToolStripStatusLabel();
            toolStrip1 = new ToolStrip();
            lblLinks = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)picMovie).BeginInit();
            panel1.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // lblInfo
            // 
            lblInfo.BackColor = Color.Black;
            lblInfo.Dock = DockStyle.Right;
            lblInfo.Font = new Font("Segoe UI", 10F);
            lblInfo.ForeColor = Color.White;
            lblInfo.Location = new Point(523, 0);
            lblInfo.Margin = new Padding(0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(385, 324);
            lblInfo.TabIndex = 5;
            lblInfo.Text = "label1";
            lblInfo.TextAlign = ContentAlignment.TopRight;
            // 
            // picMovie
            // 
            picMovie.BackColor = Color.Black;
            picMovie.Cursor = Cursors.Hand;
            picMovie.Dock = DockStyle.Fill;
            picMovie.Location = new Point(0, 0);
            picMovie.Name = "picMovie";
            picMovie.Size = new Size(523, 324);
            picMovie.SizeMode = PictureBoxSizeMode.Zoom;
            picMovie.TabIndex = 7;
            picMovie.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(statusStrip);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 324);
            panel1.Name = "panel1";
            panel1.Size = new Size(908, 137);
            panel1.TabIndex = 8;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblPercentage, lblSize, prgBar, lblFileName, lblLinks });
            statusStrip.Location = new Point(0, 111);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(908, 26);
            statusStrip.TabIndex = 8;
            statusStrip.Text = "statusStrip1";
            // 
            // lblPercentage
            // 
            lblPercentage.Name = "lblPercentage";
            lblPercentage.Size = new Size(82, 20);
            lblPercentage.Text = "Percentage";
            // 
            // lblSize
            // 
            lblSize.Name = "lblSize";
            lblSize.Size = new Size(36, 20);
            lblSize.Text = "Size";
            // 
            // prgBar
            // 
            prgBar.Name = "prgBar";
            prgBar.Size = new Size(300, 18);
            // 
            // lblFileName
            // 
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(72, 20);
            lblFileName.Text = "FileName";
            lblFileName.Click += lblFileName_Click;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(523, 25);
            toolStrip1.TabIndex = 9;
            toolStrip1.Text = "toolStrip1";
            // 
            // lblLinks
            // 
            lblLinks.Name = "lblLinks";
            lblLinks.Size = new Size(41, 20);
            lblLinks.Text = "Links";
            lblLinks.Click += lblLinks_Click;
            // 
            // frmDownloader
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(908, 461);
            Controls.Add(toolStrip1);
            Controls.Add(picMovie);
            Controls.Add(lblInfo);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmDownloader";
            Text = "Downloader";
            FormClosing += frmDownloader_FormClosing;
            Load += frmDownloader_Load;
            ((System.ComponentModel.ISupportInitialize)picMovie).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblInfo;
        private PictureBox picMovie;
        private Panel panel1;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblPercentage;
        private ToolStripStatusLabel lblSize;
        private ToolStripProgressBar prgBar;
        private ToolStripStatusLabel lblFileName;
        private ToolStrip toolStrip1;
        private ToolStripStatusLabel lblLinks;
    }
}
