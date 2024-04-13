namespace NT.IPTV
{
    partial class ChannelControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChannelControl));
            lblChannelName = new Label();
            picLogo = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // lblChannelName
            // 
            lblChannelName.AutoSize = true;
            lblChannelName.BackColor = Color.Transparent;
            lblChannelName.Dock = DockStyle.Top;
            lblChannelName.Font = new Font("Segoe UI", 8.5F);
            lblChannelName.ForeColor = Color.White;
            lblChannelName.Location = new Point(0, 0);
            lblChannelName.Name = "lblChannelName";
            lblChannelName.Padding = new Padding(0, 0, 0, 10);
            lblChannelName.Size = new Size(106, 30);
            lblChannelName.TabIndex = 0;
            lblChannelName.Text = "Channel Name";
            lblChannelName.TextAlign = ContentAlignment.TopCenter;
            // 
            // picLogo
            // 
            picLogo.BackgroundImageLayout = ImageLayout.None;
            picLogo.Cursor = Cursors.Hand;
            picLogo.Dock = DockStyle.Fill;
            picLogo.InitialImage = (Image)resources.GetObject("picLogo.InitialImage");
            picLogo.Location = new Point(0, 30);
            picLogo.Name = "picLogo";
            picLogo.Padding = new Padding(2);
            picLogo.Size = new Size(197, 166);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 1;
            picLogo.TabStop = false;
            picLogo.Click += picLogo_Click;
            picLogo.DoubleClick += picLogo_DoubleClick;
            // 
            // ChannelControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(picLogo);
            Controls.Add(lblChannelName);
            Name = "ChannelControl";
            Size = new Size(197, 196);
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblChannelName;
        private PictureBox picLogo;
    }
}
