using NT.IPTV.Models.Channel;
using NT.IPTV.Utilities;

namespace NT.IPTV
{
    partial class frmCategories
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCategories));
            pllHeader = new Panel();
            btnNameUp = new Button();
            btnRatingUp = new Button();
            btnDateUp = new Button();
            btnBigger = new Button();
            btnSmaller = new Button();
            txtSearchMovies = new SearchTextBox();
            label1 = new Label();
            txtSearch = new SearchTextBox();
            lblSeach = new Label();
            toolTip1 = new ToolTip(components);
            flwChannel = new FlowLayoutPanel();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            prgBar = new ToolStripProgressBar();
            toolStrip = new ToolStrip();
            btnLive = new ToolStripButton();
            btnMovies = new ToolStripButton();
            btnSeries = new ToolStripButton();
            btnRefresh = new ToolStripButton();
            btnGlobalSearch = new ToolStripButton();
            btnLogout = new ToolStripButton();
            flwCat = new FlowCatControl();
            pllHeader.SuspendLayout();
            statusStrip.SuspendLayout();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // pllHeader
            // 
            pllHeader.Controls.Add(btnNameUp);
            pllHeader.Controls.Add(btnRatingUp);
            pllHeader.Controls.Add(btnDateUp);
            pllHeader.Controls.Add(btnBigger);
            pllHeader.Controls.Add(btnSmaller);
            pllHeader.Controls.Add(txtSearchMovies);
            pllHeader.Controls.Add(label1);
            pllHeader.Controls.Add(txtSearch);
            pllHeader.Controls.Add(lblSeach);
            pllHeader.Dock = DockStyle.Top;
            pllHeader.Location = new Point(0, 86);
            pllHeader.Name = "pllHeader";
            pllHeader.Size = new Size(1381, 58);
            pllHeader.TabIndex = 3;
            // 
            // btnNameUp
            // 
            btnNameUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnNameUp.BackgroundImage = Properties.Resources.NameUp;
            btnNameUp.BackgroundImageLayout = ImageLayout.Zoom;
            btnNameUp.Location = new Point(1202, 11);
            btnNameUp.Name = "btnNameUp";
            btnNameUp.Size = new Size(54, 43);
            btnNameUp.TabIndex = 6;
            btnNameUp.Tag = "up";
            btnNameUp.UseVisualStyleBackColor = true;
            btnNameUp.Click += btnDown_Click;
            // 
            // btnRatingUp
            // 
            btnRatingUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRatingUp.BackgroundImage = (Image)resources.GetObject("btnRatingUp.BackgroundImage");
            btnRatingUp.BackgroundImageLayout = ImageLayout.Zoom;
            btnRatingUp.Location = new Point(1092, 11);
            btnRatingUp.Name = "btnRatingUp";
            btnRatingUp.Size = new Size(54, 43);
            btnRatingUp.TabIndex = 7;
            btnRatingUp.Tag = "down";
            btnRatingUp.UseVisualStyleBackColor = true;
            btnRatingUp.Click += btnRatingUp_Click;
            // 
            // btnDateUp
            // 
            btnDateUp.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDateUp.BackgroundImage = (Image)resources.GetObject("btnDateUp.BackgroundImage");
            btnDateUp.BackgroundImageLayout = ImageLayout.Zoom;
            btnDateUp.Location = new Point(1148, 11);
            btnDateUp.Name = "btnDateUp";
            btnDateUp.Size = new Size(54, 43);
            btnDateUp.TabIndex = 7;
            btnDateUp.Tag = "down";
            toolTip1.SetToolTip(btnDateUp, "Release date ascending");
            btnDateUp.UseVisualStyleBackColor = true;
            btnDateUp.Click += btnDateUp_Click;
            // 
            // btnBigger
            // 
            btnBigger.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBigger.BackgroundImage = (Image)resources.GetObject("btnBigger.BackgroundImage");
            btnBigger.BackgroundImageLayout = ImageLayout.Center;
            btnBigger.Location = new Point(1310, 11);
            btnBigger.Name = "btnBigger";
            btnBigger.Size = new Size(54, 43);
            btnBigger.TabIndex = 5;
            btnBigger.Tag = "*";
            btnBigger.UseVisualStyleBackColor = true;
            btnBigger.Click += btnBigger_Click;
            // 
            // btnSmaller
            // 
            btnSmaller.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSmaller.BackgroundImage = (Image)resources.GetObject("btnSmaller.BackgroundImage");
            btnSmaller.BackgroundImageLayout = ImageLayout.Center;
            btnSmaller.Location = new Point(1256, 11);
            btnSmaller.Name = "btnSmaller";
            btnSmaller.Size = new Size(54, 43);
            btnSmaller.TabIndex = 5;
            btnSmaller.Tag = "/";
            btnSmaller.UseVisualStyleBackColor = true;
            btnSmaller.Click += btnBigger_Click;
            // 
            // txtSearchMovies
            // 
            txtSearchMovies.DelayedTextChangedTimeout = 2000;
            txtSearchMovies.Location = new Point(652, 11);
            txtSearchMovies.Name = "txtSearchMovies";
            txtSearchMovies.Size = new Size(418, 27);
            txtSearchMovies.TabIndex = 4;
            txtSearchMovies.DelayedTextChanged += txtSearchMovies_DelayedTextChanged;
            txtSearchMovies.TextChanged += txtSearchMovies_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(593, 11);
            label1.Name = "label1";
            label1.Size = new Size(53, 20);
            label1.TabIndex = 3;
            label1.Text = "Search";
            // 
            // txtSearch
            // 
            txtSearch.DelayedTextChangedTimeout = 2000;
            txtSearch.Location = new Point(70, 11);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(351, 27);
            txtSearch.TabIndex = 1;
            txtSearch.DelayedTextChanged += txtSearch_DelayedTextChanged;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // lblSeach
            // 
            lblSeach.AutoSize = true;
            lblSeach.Location = new Point(11, 11);
            lblSeach.Name = "lblSeach";
            lblSeach.Size = new Size(53, 20);
            lblSeach.TabIndex = 0;
            lblSeach.Text = "Search";
            // 
            // flwChannel
            // 
            flwChannel.AutoScroll = true;
            flwChannel.BackColor = Color.FromArgb(64, 64, 64);
            flwChannel.Dock = DockStyle.Fill;
            flwChannel.Location = new Point(434, 144);
            flwChannel.Name = "flwChannel";
            flwChannel.Size = new Size(947, 495);
            flwChannel.TabIndex = 5;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblStatus, prgBar });
            statusStrip.Location = new Point(0, 639);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1381, 26);
            statusStrip.TabIndex = 7;
            statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(49, 20);
            lblStatus.Text = "Status";
            // 
            // prgBar
            // 
            prgBar.MarqueeAnimationSpeed = 30;
            prgBar.Name = "prgBar";
            prgBar.Size = new Size(300, 18);
            prgBar.Style = ProgressBarStyle.Marquee;
            // 
            // toolStrip
            // 
            toolStrip.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip.ImageScalingSize = new Size(59, 59);
            toolStrip.Items.AddRange(new ToolStripItem[] { btnLive, btnMovies, btnSeries, btnRefresh, btnGlobalSearch, btnSettings2, btnLogout });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(1381, 86);
            toolStrip.TabIndex = 9;
            toolStrip.Text = "toolStrip1";
            // 
            // btnLive
            // 
            btnLive.Checked = true;
            btnLive.CheckOnClick = true;
            btnLive.CheckState = CheckState.Checked;
            btnLive.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnLive.Image = (Image)resources.GetObject("btnLive.Image");
            btnLive.Name = "btnLive";
            btnLive.Padding = new Padding(10);
            btnLive.Size = new Size(83, 83);
            btnLive.Text = "Channels";
            btnLive.ToolTipText = "Channels - Live TV";
            btnLive.Click += btnLive_Click;
            // 
            // btnMovies
            // 
            btnMovies.CheckOnClick = true;
            btnMovies.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMovies.Image = (Image)resources.GetObject("btnMovies.Image");
            btnMovies.Name = "btnMovies";
            btnMovies.Padding = new Padding(10);
            btnMovies.Size = new Size(83, 83);
            btnMovies.Text = "Movies";
            btnMovies.ToolTipText = "Movies";
            btnMovies.Click += btnLive_Click;
            // 
            // btnSeries
            // 
            btnSeries.CheckOnClick = true;
            btnSeries.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSeries.Image = (Image)resources.GetObject("btnSeries.Image");
            btnSeries.Name = "btnSeries";
            btnSeries.Padding = new Padding(10);
            btnSeries.Size = new Size(83, 83);
            btnSeries.Text = "Series";
            btnSeries.ToolTipText = "Series";
            btnSeries.Click += btnLive_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnRefresh.Image = (Image)resources.GetObject("btnRefresh.Image");
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Padding = new Padding(10);
            btnRefresh.Size = new Size(83, 83);
            btnRefresh.Text = "Refresh";
            btnRefresh.ToolTipText = "Refresh Content";
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnGlobalSearch
            // 
            btnGlobalSearch.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnGlobalSearch.Image = (Image)resources.GetObject("btnGlobalSearch.Image");
            btnGlobalSearch.Name = "btnGlobalSearch";
            btnGlobalSearch.Padding = new Padding(10);
            btnGlobalSearch.Size = new Size(83, 83);
            btnGlobalSearch.Text = "Global Search";
            btnGlobalSearch.ToolTipText = "Search All Content";
            btnGlobalSearch.Click += btnGlobalSearch_Click;
            // 
            // btnLogout
            // 
            btnLogout.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnLogout.Image = (Image)resources.GetObject("btnLogout.Image");
            btnLogout.Name = "btnLogout";
            btnLogout.Padding = new Padding(10);
            btnLogout.Size = new Size(83, 83);
            btnLogout.Text = "Log Out";
            btnLogout.ToolTipText = "Log Out & Disable Auto-Login";
            btnLogout.Click += btnLogout_Click;
            // 
            // btnSettings2
            // 
            btnSettings2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSettings2.Image = (Image)resources.GetObject("btnSettings2.Image");
            btnSettings2.ImageScaling = ToolStripItemImageScaling.SizeToFit;
            btnSettings2.Name = "btnSettings2";
            btnSettings2.Padding = new Padding(10);
            btnSettings2.Size = new Size(83, 83);
            btnSettings2.Text = "Settings";
            btnSettings2.ToolTipText = "Application Settings";
            btnSettings2.Click += btnSettings_Click;
            // 
            // flwCat
            // 
            flwCat.AutoScroll = true;
            flwCat.BackColor = Color.FromArgb(64, 64, 64);
            flwCat.Categories = null;
            flwCat.Dock = DockStyle.Left;
            flwCat.Location = new Point(0, 144);
            flwCat.Name = "flwCat";
            flwCat.SelectedItem = null;
            flwCat.Size = new Size(434, 495);
            flwCat.TabIndex = 10;
            flwCat.SelectedIndexChaged += lstCategories_SelectedIndexChanged;
            flwCat.Load += flwCat_Load;
            // 
            // frmCategories
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1381, 665);
            Controls.Add(flwChannel);
            Controls.Add(flwCat);
            Controls.Add(pllHeader);
            Controls.Add(statusStrip);
            Controls.Add(toolStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmCategories";
            Text = "NT.IPTV";
            WindowState = FormWindowState.Maximized;
            FormClosing += frmCategories_FormClosing;
            Load += frmCategories_Load;
            pllHeader.ResumeLayout(false);
            pllHeader.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            toolStrip.ResumeLayout(false);
            toolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel pllHeader;
        private SearchTextBox txtSearch;
        private Label lblSeach;
        private ToolTip toolTip1;
        private SearchTextBox txtSearchMovies;
        private Label label1;
        private FlowLayoutPanel flwChannel;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private ToolStripProgressBar prgBar;
        private ToolStrip toolStrip;
        private ToolStripButton btnLive;
        private ToolStripButton btnMovies;
        private ToolStripButton btnSeries;
        private Button btnBigger;
        private Button btnSmaller;
        private Button btnNameUp;
        private Button btnDateUp;
        private Button btnRatingUp;
        private FlowCatControl flwCat;
        private ToolStripButton btnGlobalSearch;
        private ToolStripButton btnRefresh;
        private ToolStripButton btnLogout;
        private ToolStripButton btnSettings2;
    }
}
