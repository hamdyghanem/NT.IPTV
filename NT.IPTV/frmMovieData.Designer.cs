namespace NT.IPTV
{
    partial class frmMovieData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMovieData));
            lblInfo = new Label();
            tabSeries = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            pnlHeader = new Panel();
            lblCast = new Label();
            lblData = new Label();
            picCover = new PictureBox();
            picMovie = new PicBackdropPath();
            MyToolTip = new ToolStrip();
            btnOpenInVLC = new ToolStripButton();
            btnWatchTrailer = new ToolStripButton();
            btnDownloadLinks = new ToolStripButton();
            btnDownload = new ToolStripButton();
            btnDownloadSeries = new ToolStripDropDownButton();
            btnDownloadAllSeasons = new ToolStripMenuItem();
            btnClose = new ToolStripButton();
            btnCancel = new Button();
            tabSeries.SuspendLayout();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCover).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picMovie).BeginInit();
            MyToolTip.SuspendLayout();
            SuspendLayout();
            // 
            // lblInfo
            // 
            lblInfo.BackColor = Color.Transparent;
            lblInfo.Dock = DockStyle.Fill;
            lblInfo.Font = new Font("Segoe UI", 18F);
            lblInfo.ForeColor = Color.White;
            lblInfo.Location = new Point(201, 0);
            lblInfo.Margin = new Padding(0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(894, 131);
            lblInfo.TabIndex = 3;
            lblInfo.Text = "label1";
            lblInfo.TextAlign = ContentAlignment.TopRight;
            // 
            // tabSeries
            // 
            tabSeries.Appearance = TabAppearance.FlatButtons;
            tabSeries.Controls.Add(tabPage1);
            tabSeries.Controls.Add(tabPage2);
            tabSeries.Font = new Font("Segoe UI", 16F);
            tabSeries.Location = new Point(74, 240);
            tabSeries.Margin = new Padding(3, 2, 3, 2);
            tabSeries.Name = "tabSeries";
            tabSeries.SelectedIndex = 0;
            tabSeries.Size = new Size(356, 138);
            tabSeries.TabIndex = 5;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Black;
            tabPage1.Location = new Point(4, 42);
            tabPage1.Margin = new Padding(3, 2, 3, 2);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3, 2, 3, 2);
            tabPage1.Size = new Size(348, 92);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 42);
            tabPage2.Margin = new Padding(3, 2, 3, 2);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3, 2, 3, 2);
            tabPage2.Size = new Size(348, 92);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.Black;
            pnlHeader.Controls.Add(lblInfo);
            pnlHeader.Controls.Add(lblCast);
            pnlHeader.Controls.Add(lblData);
            pnlHeader.Controls.Add(picCover);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 82);
            pnlHeader.Margin = new Padding(3, 2, 3, 2);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(0, 0, 0, 4);
            pnlHeader.Size = new Size(1095, 241);
            pnlHeader.TabIndex = 6;
            // 
            // lblCast
            // 
            lblCast.BackColor = Color.Transparent;
            lblCast.Dock = DockStyle.Bottom;
            lblCast.Font = new Font("Segoe UI", 18F);
            lblCast.ForeColor = Color.Bisque;
            lblCast.Location = new Point(201, 131);
            lblCast.Margin = new Padding(0);
            lblCast.Name = "lblCast";
            lblCast.Size = new Size(894, 56);
            lblCast.TabIndex = 6;
            lblCast.Text = "label1";
            lblCast.TextAlign = ContentAlignment.TopRight;
            // 
            // lblData
            // 
            lblData.BackColor = Color.Transparent;
            lblData.Dock = DockStyle.Bottom;
            lblData.Font = new Font("Segoe UI", 12F);
            lblData.ForeColor = Color.White;
            lblData.Location = new Point(201, 187);
            lblData.Margin = new Padding(0);
            lblData.Name = "lblData";
            lblData.Size = new Size(894, 50);
            lblData.TabIndex = 7;
            lblData.Text = "label1";
            lblData.TextAlign = ContentAlignment.TopRight;
            // 
            // picCover
            // 
            picCover.BackColor = Color.Black;
            picCover.BorderStyle = BorderStyle.FixedSingle;
            picCover.Cursor = Cursors.Hand;
            picCover.Dock = DockStyle.Left;
            picCover.Location = new Point(0, 0);
            picCover.Margin = new Padding(3, 2, 3, 2);
            picCover.Name = "picCover";
            picCover.Padding = new Padding(4);
            picCover.Size = new Size(201, 237);
            picCover.SizeMode = PictureBoxSizeMode.Zoom;
            picCover.TabIndex = 5;
            picCover.TabStop = false;
            // 
            // picMovie
            // 
            picMovie.BackColor = Color.Black;
            picMovie.BackdropPath = null;
            picMovie.Cursor = Cursors.Hand;
            picMovie.DelayedTextChangedTimeout = 2000;
            picMovie.Location = new Point(441, 287);
            picMovie.Margin = new Padding(3, 2, 3, 2);
            picMovie.Name = "picMovie";
            picMovie.Size = new Size(509, 91);
            picMovie.SizeMode = PictureBoxSizeMode.Zoom;
            picMovie.TabIndex = 9;
            picMovie.TabStop = false;
            picMovie.Click += picMovie_Click;
            // 
            // MyToolTip
            // 
            MyToolTip.ImageScalingSize = new Size(55, 55);
            MyToolTip.Items.AddRange(new ToolStripItem[] { btnOpenInVLC, btnWatchTrailer, btnDownloadLinks, btnDownload, btnDownloadSeries, btnClose });
            MyToolTip.Location = new Point(0, 0);
            MyToolTip.Name = "MyToolTip";
            MyToolTip.Size = new Size(1095, 82);
            MyToolTip.TabIndex = 10;
            MyToolTip.Text = "toolStrip1";
            // 
            // btnOpenInVLC
            // 
            btnOpenInVLC.Checked = true;
            btnOpenInVLC.CheckOnClick = true;
            btnOpenInVLC.CheckState = CheckState.Checked;
            btnOpenInVLC.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnOpenInVLC.Image = (Image)resources.GetObject("btnOpenInVLC.Image");
            btnOpenInVLC.ImageTransparentColor = Color.Magenta;
            btnOpenInVLC.Name = "btnOpenInVLC";
            btnOpenInVLC.Padding = new Padding(10);
            btnOpenInVLC.Size = new Size(79, 79);
            btnOpenInVLC.Text = "Open in VLC";
            btnOpenInVLC.ToolTipText = "Open in VLC";
            btnOpenInVLC.Click += btnOpenVLC_Click;
            // 
            // btnWatchTrailer
            // 
            btnWatchTrailer.CheckOnClick = true;
            btnWatchTrailer.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnWatchTrailer.Image = (Image)resources.GetObject("btnWatchTrailer.Image");
            btnWatchTrailer.ImageTransparentColor = Color.Magenta;
            btnWatchTrailer.Name = "btnWatchTrailer";
            btnWatchTrailer.Padding = new Padding(10);
            btnWatchTrailer.Size = new Size(79, 79);
            btnWatchTrailer.Text = "Watch Trailer";
            btnWatchTrailer.ToolTipText = "Watch Trailer";
            btnWatchTrailer.Click += btnWatchTrailer_Click;
            // 
            // btnDownloadLinks
            // 
            btnDownloadLinks.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDownloadLinks.Image = (Image)resources.GetObject("btnDownloadLinks.Image");
            btnDownloadLinks.ImageTransparentColor = Color.Magenta;
            btnDownloadLinks.Name = "btnDownloadLinks";
            btnDownloadLinks.Size = new Size(59, 79);
            btnDownloadLinks.Text = "Download Links";
            btnDownloadLinks.Click += btnDownloadLinks_Click;
            // 
            // btnDownload
            // 
            btnDownload.CheckOnClick = true;
            btnDownload.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDownload.Image = (Image)resources.GetObject("btnDownload.Image");
            btnDownload.ImageTransparentColor = Color.Magenta;
            btnDownload.Name = "btnDownload";
            btnDownload.Padding = new Padding(10);
            btnDownload.Size = new Size(79, 79);
            btnDownload.Text = "Download";
            btnDownload.ToolTipText = "Download";
            btnDownload.Click += btnDownload_Click;
            // 
            // btnDownloadSeries
            // 
            btnDownloadSeries.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnDownloadSeries.DropDownItems.AddRange(new ToolStripItem[] { btnDownloadAllSeasons });
            btnDownloadSeries.Image = Properties.Resources.download_1915753_640;
            btnDownloadSeries.ImageTransparentColor = Color.Magenta;
            btnDownloadSeries.Name = "btnDownloadSeries";
            btnDownloadSeries.Padding = new Padding(10);
            btnDownloadSeries.Size = new Size(88, 79);
            // 
            // btnDownloadAllSeasons
            // 
            btnDownloadAllSeasons.Image = Properties.Resources.download;
            btnDownloadAllSeasons.Name = "btnDownloadAllSeasons";
            btnDownloadAllSeasons.Size = new Size(229, 62);
            btnDownloadAllSeasons.Text = "Download All Seasons";
            btnDownloadAllSeasons.Click += btnDownloadSeries_Click;
            // 
            // btnClose
            // 
            btnClose.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnClose.Image = (Image)resources.GetObject("btnClose.Image");
            btnClose.ImageTransparentColor = Color.Magenta;
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(59, 79);
            btnClose.Text = "Close";
            btnClose.Click += btnClose_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(0, 0);
            btnCancel.Margin = new Padding(3, 2, 3, 2);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(82, 22);
            btnCancel.TabIndex = 11;
            btnCancel.Text = "Close";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Visible = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // frmMovieData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(1095, 458);
            Controls.Add(btnCancel);
            Controls.Add(picMovie);
            Controls.Add(tabSeries);
            Controls.Add(pnlHeader);
            Controls.Add(MyToolTip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Margin = new Padding(3, 2, 3, 2);
            Name = "frmMovieData";
            Text = "Data";
            WindowState = FormWindowState.Maximized;
            FormClosing += frmMovieData_FormClosing;
            Load += frmMovieData_Load;
            tabSeries.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCover).EndInit();
            ((System.ComponentModel.ISupportInitialize)picMovie).EndInit();
            MyToolTip.ResumeLayout(false);
            MyToolTip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel pllHeader;
        private Button btnOpenVLC;
        private Label lblInfo;
        private TabControl tabSeries;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel pnlHeader;
        private PictureBox picCover;
        private Label lblCast;
        private Label lblData;
        private PicBackdropPath picMovie;
        private ToolStrip MyToolTip;
        private ToolStripButton btnOpenInVLC;
        private ToolStripButton btnDownload;
        private ToolStripButton btnWatchTrailer;
        private ToolStripButton btnDownloadLinks;
        private ToolStripButton btnClose;
        private Button btnCancel;
        private ToolStripDropDownButton btnDownloadSeries;
        private ToolStripMenuItem btnDownloadAllSeasons;
    }
}
