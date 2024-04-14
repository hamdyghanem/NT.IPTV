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
            btnDownload = new ToolStripButton();
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
            lblInfo.Location = new Point(229, 0);
            lblInfo.Margin = new Padding(0);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(882, 174);
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
            tabSeries.Location = new Point(84, 320);
            tabSeries.Name = "tabSeries";
            tabSeries.SelectedIndex = 0;
            tabSeries.Size = new Size(407, 184);
            tabSeries.TabIndex = 5;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = Color.Black;
            tabPage1.Location = new Point(4, 49);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(399, 131);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 49);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(399, 131);
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
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Padding = new Padding(0, 0, 0, 5);
            pnlHeader.Size = new Size(1111, 321);
            pnlHeader.TabIndex = 6;
            // 
            // lblCast
            // 
            lblCast.BackColor = Color.Transparent;
            lblCast.Dock = DockStyle.Bottom;
            lblCast.Font = new Font("Segoe UI", 18F);
            lblCast.ForeColor = Color.Bisque;
            lblCast.Location = new Point(229, 174);
            lblCast.Margin = new Padding(0);
            lblCast.Name = "lblCast";
            lblCast.Size = new Size(882, 75);
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
            lblData.Location = new Point(229, 249);
            lblData.Margin = new Padding(0);
            lblData.Name = "lblData";
            lblData.Size = new Size(882, 67);
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
            picCover.Name = "picCover";
            picCover.Padding = new Padding(5);
            picCover.Size = new Size(229, 316);
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
            picMovie.Location = new Point(504, 383);
            picMovie.Name = "picMovie";
            picMovie.Size = new Size(582, 121);
            picMovie.SizeMode = PictureBoxSizeMode.Zoom;
            picMovie.TabIndex = 9;
            picMovie.TabStop = false;
            picMovie.Click += picMovie_Click;
            // 
            // MyToolTip
            // 
            MyToolTip.ImageScalingSize = new Size(55, 55);
            MyToolTip.Items.AddRange(new ToolStripItem[] { btnOpenInVLC, btnWatchTrailer, btnDownload });
            MyToolTip.Location = new Point(0, 0);
            MyToolTip.Name = "MyToolTip";
            MyToolTip.Size = new Size(1111, 82);
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
            // frmMovieData
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1111, 516);
            Controls.Add(picMovie);
            Controls.Add(tabSeries);
            Controls.Add(pnlHeader);
            Controls.Add(MyToolTip);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
    }
}
