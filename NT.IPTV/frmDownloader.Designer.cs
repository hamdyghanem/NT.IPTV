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
            lblPercentage = new Label();
            lblSize = new Label();
            progressBar1 = new ProgressBar();
            lblInfo = new Label();
            picMovie = new PictureBox();
            panel1 = new Panel();
            lblFileName = new Label();
            ((System.ComponentModel.ISupportInitialize)picMovie).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // lblPercentage
            // 
            lblPercentage.Anchor = AnchorStyles.Bottom;
            lblPercentage.AutoSize = true;
            lblPercentage.BackColor = Color.Transparent;
            lblPercentage.Location = new Point(12, 41);
            lblPercentage.Name = "lblPercentage";
            lblPercentage.Size = new Size(29, 20);
            lblPercentage.TabIndex = 1;
            lblPercentage.Text = "0%";
            // 
            // lblSize
            // 
            lblSize.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblSize.AutoSize = true;
            lblSize.Location = new Point(60, 41);
            lblSize.Name = "lblSize";
            lblSize.Size = new Size(36, 20);
            lblSize.TabIndex = 1;
            lblSize.Text = "Size";
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Bottom;
            progressBar1.Location = new Point(0, 432);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(908, 29);
            progressBar1.TabIndex = 2;
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
            lblInfo.Size = new Size(385, 371);
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
            picMovie.Size = new Size(523, 371);
            picMovie.SizeMode = PictureBoxSizeMode.Zoom;
            picMovie.TabIndex = 7;
            picMovie.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(lblFileName);
            panel1.Controls.Add(lblSize);
            panel1.Controls.Add(lblPercentage);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 371);
            panel1.Name = "panel1";
            panel1.Size = new Size(908, 61);
            panel1.TabIndex = 8;
            // 
            // lblFileName
            // 
            lblFileName.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblFileName.AutoSize = true;
            lblFileName.Cursor = Cursors.Hand;
            lblFileName.Location = new Point(12, 3);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(72, 20);
            lblFileName.TabIndex = 1;
            lblFileName.Text = "FileName";
            lblFileName.Click += lblFileName_Click;
            // 
            // frmDownloader
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(908, 461);
            Controls.Add(picMovie);
            Controls.Add(lblInfo);
            Controls.Add(panel1);
            Controls.Add(progressBar1);
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
            ResumeLayout(false);
        }

        #endregion
        private Label lblPercentage;
        private Label lblSize;
        private ProgressBar progressBar1;
        private Label lblInfo;
        private PictureBox picMovie;
        private Panel panel1;
        private Label lblFileName;
    }
}
