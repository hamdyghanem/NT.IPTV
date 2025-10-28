namespace NT.IPTV
{
    partial class RowSeriesControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RowSeriesControl));
            lblName = new Label();
            picLogo = new PictureBox();
            lblPlot = new Label();
            lblDuration = new Label();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.BackColor = Color.Transparent;
            lblName.Dock = DockStyle.Top;
            lblName.Font = new Font("Segoe UI", 12F);
            lblName.ForeColor = Color.FromArgb(255, 255, 192);
            lblName.Location = new Point(106, 0);
            lblName.Name = "lblName";
            lblName.Padding = new Padding(5);
            lblName.Size = new Size(539, 38);
            lblName.TabIndex = 0;
            lblName.Text = "Series Name";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            lblName.MouseEnter += lblDuration_MouseEnter;
            lblName.MouseLeave += lblDuration_MouseLeave;
            // 
            // picLogo
            // 
            picLogo.BackgroundImageLayout = ImageLayout.None;
            picLogo.BorderStyle = BorderStyle.FixedSingle;
            picLogo.Cursor = Cursors.Hand;
            picLogo.Dock = DockStyle.Left;
            picLogo.InitialImage = (Image)resources.GetObject("picLogo.InitialImage");
            picLogo.Location = new Point(0, 0);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(106, 111);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 1;
            picLogo.TabStop = false;
            picLogo.Click += picLogo_Click;
            picLogo.DoubleClick += picLogo_DoubleClick;
            // 
            // lblPlot
            // 
            lblPlot.BackColor = Color.Transparent;
            lblPlot.Dock = DockStyle.Top;
            lblPlot.Font = new Font("Segoe UI", 10F);
            lblPlot.ForeColor = Color.White;
            lblPlot.Location = new Point(106, 38);
            lblPlot.Name = "lblPlot";
            lblPlot.Padding = new Padding(5);
            lblPlot.Size = new Size(539, 33);
            lblPlot.TabIndex = 2;
            lblPlot.Text = "Series Name";
            lblPlot.TextAlign = ContentAlignment.MiddleLeft;
            lblPlot.MouseEnter += lblDuration_MouseEnter;
            lblPlot.MouseLeave += lblDuration_MouseLeave;
            // 
            // lblDuration
            // 
            lblDuration.BackColor = Color.Transparent;
            lblDuration.Dock = DockStyle.Top;
            lblDuration.Font = new Font("Segoe UI", 10F);
            lblDuration.ForeColor = Color.White;
            lblDuration.Location = new Point(106, 71);
            lblDuration.Name = "lblDuration";
            lblDuration.Padding = new Padding(5);
            lblDuration.Size = new Size(539, 33);
            lblDuration.TabIndex = 3;
            lblDuration.Text = "0:0:0";
            lblDuration.TextAlign = ContentAlignment.MiddleLeft;
            lblDuration.Click += lblDuration_Click;
            lblDuration.MouseEnter += lblDuration_MouseEnter;
            lblDuration.MouseLeave += lblDuration_MouseLeave;
            // 
            // RowSeriesControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(lblDuration);
            Controls.Add(lblPlot);
            Controls.Add(lblName);
            Controls.Add(picLogo);
            Name = "RowSeriesControl";
            Size = new Size(645, 111);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Label lblName;
        private PictureBox picLogo;
        private Label lblPlot;
        private Label lblDuration;
    }
}
