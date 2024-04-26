namespace NT.IPTV
{
    partial class frmGlobalSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGlobalSearch));
            MyToolTip = new ToolTip(components);
            statusStrip1 = new StatusStrip();
            prgBar = new ToolStripProgressBar();
            lblFound = new ToolStripStatusLabel();
            txtSearchMovies = new Models.Channel.SearchTextBox();
            label1 = new Label();
            btnCancel = new Button();
            btnOk = new Button();
            lstGlobalSearch = new ListBox();
            pnlHeader = new Panel();
            pnlBottom = new Panel();
            statusStrip1.SuspendLayout();
            pnlHeader.SuspendLayout();
            pnlBottom.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { prgBar, lblFound });
            statusStrip1.Location = new Point(0, 306);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(809, 26);
            statusStrip1.TabIndex = 11;
            statusStrip1.Text = "statusStrip1";
            // 
            // prgBar
            // 
            prgBar.Name = "prgBar";
            prgBar.Size = new Size(200, 18);
            // 
            // lblFound
            // 
            lblFound.Name = "lblFound";
            lblFound.Size = new Size(17, 20);
            lblFound.Text = "0";
            // 
            // txtSearchMovies
            // 
            txtSearchMovies.DelayedTextChangedTimeout = 2000;
            txtSearchMovies.Location = new Point(72, 16);
            txtSearchMovies.Name = "txtSearchMovies";
            txtSearchMovies.Size = new Size(680, 27);
            txtSearchMovies.TabIndex = 0;
            txtSearchMovies.DelayedTextChanged += txtSearchMovies_DelayedTextChanged;
            txtSearchMovies.TextChanged += txtSearchMovies_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(13, 16);
            label1.Name = "label1";
            label1.Size = new Size(53, 20);
            label1.TabIndex = 12;
            label1.Text = "Search";
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(562, 8);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(117, 36);
            btnCancel.TabIndex = 16;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnOk
            // 
            btnOk.Location = new Point(685, 8);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(117, 36);
            btnOk.TabIndex = 1;
            btnOk.Text = "Ok";
            btnOk.UseVisualStyleBackColor = true;
            btnOk.Click += btnOk_Click;
            // 
            // lstGlobalSearch
            // 
            lstGlobalSearch.Dock = DockStyle.Fill;
            lstGlobalSearch.FormattingEnabled = true;
            lstGlobalSearch.Location = new Point(0, 51);
            lstGlobalSearch.Name = "lstGlobalSearch";
            lstGlobalSearch.Size = new Size(809, 205);
            lstGlobalSearch.TabIndex = 17;
            lstGlobalSearch.SelectedIndexChanged += lstGlobalSearch_SelectedIndexChanged;
            lstGlobalSearch.DoubleClick += lstGlobalSearch_DoubleClick;
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(txtSearchMovies);
            pnlHeader.Controls.Add(label1);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(809, 51);
            pnlHeader.TabIndex = 18;
            // 
            // pnlBottom
            // 
            pnlBottom.Controls.Add(btnCancel);
            pnlBottom.Controls.Add(btnOk);
            pnlBottom.Dock = DockStyle.Bottom;
            pnlBottom.Location = new Point(0, 256);
            pnlBottom.Name = "pnlBottom";
            pnlBottom.Size = new Size(809, 50);
            pnlBottom.TabIndex = 18;
            // 
            // frmGlobalSearch
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Gray;
            CancelButton = btnCancel;
            ClientSize = new Size(809, 332);
            Controls.Add(lstGlobalSearch);
            Controls.Add(pnlBottom);
            Controls.Add(pnlHeader);
            Controls.Add(statusStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmGlobalSearch";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Global Search";
            FormClosing += frmGlobalSearch_FormClosing;
            Load += frmGlobalSearch_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            pnlBottom.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolTip MyToolTip;
        private StatusStrip statusStrip1;
        private ToolStripProgressBar prgBar;
        private Models.Channel.SearchTextBox txtSearchMovies;
        private Label label1;
        private Button btnCancel;
        private Button btnOk;
        private ListBox lstGlobalSearch;
        private ToolStripStatusLabel lblFound;
        private Panel pnlHeader;
        private Panel pnlBottom;
    }
}
