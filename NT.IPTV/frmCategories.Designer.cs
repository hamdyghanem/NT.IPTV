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
            lstCategories = new ListBox();
            pllHeader = new Panel();
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
            pllHeader.SuspendLayout();
            statusStrip.SuspendLayout();
            toolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // lstCategories
            // 
            lstCategories.Dock = DockStyle.Left;
            lstCategories.FormattingEnabled = true;
            lstCategories.Location = new Point(0, 140);
            lstCategories.Name = "lstCategories";
            lstCategories.Size = new Size(457, 499);
            lstCategories.TabIndex = 2;
            lstCategories.SelectedIndexChanged += lstCategories_SelectedIndexChanged;
            lstCategories.SelectedValueChanged += lstCategories_SelectedValueChanged;
            // 
            // pllHeader
            // 
            pllHeader.Controls.Add(btnBigger);
            pllHeader.Controls.Add(btnSmaller);
            pllHeader.Controls.Add(txtSearchMovies);
            pllHeader.Controls.Add(label1);
            pllHeader.Controls.Add(txtSearch);
            pllHeader.Controls.Add(lblSeach);
            pllHeader.Dock = DockStyle.Top;
            pllHeader.Location = new Point(0, 82);
            pllHeader.Name = "pllHeader";
            pllHeader.Size = new Size(1381, 58);
            pllHeader.TabIndex = 3;
            // 
            // btnBigger
            // 
            btnBigger.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBigger.BackgroundImage = (Image)resources.GetObject("btnBigger.BackgroundImage");
            btnBigger.BackgroundImageLayout = ImageLayout.Center;
            btnBigger.Location = new Point(1310, 9);
            btnBigger.Name = "btnBigger";
            btnBigger.Size = new Size(66, 43);
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
            btnSmaller.Location = new Point(1238, 12);
            btnSmaller.Name = "btnSmaller";
            btnSmaller.Size = new Size(66, 43);
            btnSmaller.TabIndex = 5;
            btnSmaller.Tag = "/";
            btnSmaller.UseVisualStyleBackColor = true;
            btnSmaller.Click += btnBigger_Click;
            // 
            // txtSearchMovies
            // 
            txtSearchMovies.DelayedTextChangedTimeout = 2000;
            txtSearchMovies.Location = new Point(547, 11);
            txtSearchMovies.Name = "txtSearchMovies";
            txtSearchMovies.Size = new Size(418, 27);
            txtSearchMovies.TabIndex = 4;
            txtSearchMovies.DelayedTextChanged += txtSearchMovies_DelayedTextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(466, 11);
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
            txtSearch.Size = new Size(262, 27);
            txtSearch.TabIndex = 1;
            txtSearch.DelayedTextChanged += txtSearch_DelayedTextChanged;
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
            flwChannel.Location = new Point(457, 140);
            flwChannel.Name = "flwChannel";
            flwChannel.Size = new Size(924, 499);
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
            prgBar.Name = "prgBar";
            prgBar.Size = new Size(100, 18);
            // 
            // toolStrip
            // 
            toolStrip.ImageScalingSize = new Size(55, 55);
            toolStrip.Items.AddRange(new ToolStripItem[] { btnLive, btnMovies, btnSeries });
            toolStrip.Location = new Point(0, 0);
            toolStrip.Name = "toolStrip";
            toolStrip.Size = new Size(1381, 82);
            toolStrip.TabIndex = 9;
            toolStrip.Text = "toolStrip1";
            // 
            // btnLive
            // 
            btnLive.Checked = true;
            btnLive.CheckOnClick = true;
            btnLive.CheckState = CheckState.Checked;
            btnLive.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnLive.Image = Properties.Resources._4668380_1_;
            btnLive.ImageTransparentColor = Color.Magenta;
            btnLive.Name = "btnLive";
            btnLive.Padding = new Padding(10);
            btnLive.Size = new Size(79, 79);
            btnLive.Text = "toolStripButton1";
            btnLive.ToolTipText = "Channels";
            btnLive.Click += btnLive_Click;
            // 
            // btnMovies
            // 
            btnMovies.CheckOnClick = true;
            btnMovies.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnMovies.Image = Properties.Resources._2917656_1_;
            btnMovies.ImageTransparentColor = Color.Magenta;
            btnMovies.Name = "btnMovies";
            btnMovies.Padding = new Padding(10);
            btnMovies.Size = new Size(79, 79);
            btnMovies.Text = "btnSeries";
            btnMovies.ToolTipText = "Movies";
            btnMovies.Click += btnLive_Click;
            // 
            // btnSeries
            // 
            btnSeries.CheckOnClick = true;
            btnSeries.DisplayStyle = ToolStripItemDisplayStyle.Image;
            btnSeries.Image = Properties.Resources._6553523_1_;
            btnSeries.ImageTransparentColor = Color.Magenta;
            btnSeries.Name = "btnSeries";
            btnSeries.Padding = new Padding(10);
            btnSeries.Size = new Size(79, 79);
            btnSeries.Text = "btnSeries";
            btnSeries.ToolTipText = "Series";
            btnSeries.Click += btnLive_Click;
            // 
            // frmCategories
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1381, 665);
            Controls.Add(flwChannel);
            Controls.Add(lstCategories);
            Controls.Add(pllHeader);
            Controls.Add(statusStrip);
            Controls.Add(toolStrip);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmCategories";
            Text = "Categories";
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

        private ListBox lstCategories;
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
    }
}
