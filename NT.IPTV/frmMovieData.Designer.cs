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
            pllHeader = new Panel();
            btnWatchTrailer = new Button();
            btnDownloadViaWeb = new Button();
            btnDownload = new Button();
            btnOpenVLC = new Button();
            lblInfo = new Label();
            tabSeries = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            pnlHeader = new Panel();
            lblCast = new Label();
            lblData = new Label();
            picCover = new PictureBox();
            picMovie = new PicBackdropPath();
            pllHeader.SuspendLayout();
            tabSeries.SuspendLayout();
            pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picCover).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picMovie).BeginInit();
            SuspendLayout();
            // 
            // pllHeader
            // 
            pllHeader.Controls.Add(btnWatchTrailer);
            pllHeader.Controls.Add(btnDownloadViaWeb);
            pllHeader.Controls.Add(btnDownload);
            pllHeader.Controls.Add(btnOpenVLC);
            pllHeader.Dock = DockStyle.Top;
            pllHeader.Location = new Point(0, 0);
            pllHeader.Name = "pllHeader";
            pllHeader.Size = new Size(1111, 56);
            pllHeader.TabIndex = 1;
            // 
            // btnWatchTrailer
            // 
            btnWatchTrailer.Location = new Point(504, 12);
            btnWatchTrailer.Name = "btnWatchTrailer";
            btnWatchTrailer.Size = new Size(153, 29);
            btnWatchTrailer.TabIndex = 0;
            btnWatchTrailer.Text = "Watch Trailer";
            btnWatchTrailer.UseVisualStyleBackColor = true;
            btnWatchTrailer.Click += btnWatchTrailer_Click;
            // 
            // btnDownloadViaWeb
            // 
            btnDownloadViaWeb.Location = new Point(345, 12);
            btnDownloadViaWeb.Name = "btnDownloadViaWeb";
            btnDownloadViaWeb.Size = new Size(153, 29);
            btnDownloadViaWeb.TabIndex = 0;
            btnDownloadViaWeb.Text = "Download Via Web";
            btnDownloadViaWeb.UseVisualStyleBackColor = true;
            btnDownloadViaWeb.Click += btnDownloadViaWeb_Click;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(186, 12);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(153, 29);
            btnDownload.TabIndex = 0;
            btnDownload.Text = "Download";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // btnOpenVLC
            // 
            btnOpenVLC.Location = new Point(27, 12);
            btnOpenVLC.Name = "btnOpenVLC";
            btnOpenVLC.Size = new Size(153, 29);
            btnOpenVLC.TabIndex = 0;
            btnOpenVLC.Text = "Open in VLC";
            btnOpenVLC.UseVisualStyleBackColor = true;
            btnOpenVLC.Click += btnOpenVLC_Click;
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
            pnlHeader.Location = new Point(0, 56);
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
            picMovie.Location = new Point(610, 383);
            picMovie.Name = "picMovie";
            picMovie.Size = new Size(330, 121);
            picMovie.SizeMode = PictureBoxSizeMode.Zoom;
            picMovie.TabIndex = 9;
            picMovie.TabStop = false;
            picMovie.Click += picMovie_Click;
            // 
            // frmMovieData
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1111, 516);
            Controls.Add(picMovie);
            Controls.Add(tabSeries);
            Controls.Add(pnlHeader);
            Controls.Add(pllHeader);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmMovieData";
            Text = "Data";
            WindowState = FormWindowState.Maximized;
            FormClosing += frmMovieData_FormClosing;
            Load += frmMovieData_Load;
            pllHeader.ResumeLayout(false);
            tabSeries.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picCover).EndInit();
            ((System.ComponentModel.ISupportInitialize)picMovie).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel pllHeader;
        private Button btnOpenVLC;
        private Label lblInfo;
        private Button btnDownload;
        private TabControl tabSeries;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Panel pnlHeader;
        private PictureBox picCover;
        private Label lblCast;
        private Label lblData;
        private PicBackdropPath picMovie;
        private Button btnWatchTrailer;
        private Button btnDownloadViaWeb;
    }
}
