namespace NT.IPTV
{
    partial class frmStream
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStream));
            axWindowsMediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer).BeginInit();
            SuspendLayout();
            // 
            // axWindowsMediaPlayer
            // 
            axWindowsMediaPlayer.Dock = DockStyle.Fill;
            axWindowsMediaPlayer.Enabled = true;
            axWindowsMediaPlayer.Location = new Point(0, 0);
            axWindowsMediaPlayer.Name = "axWindowsMediaPlayer";
            axWindowsMediaPlayer.OcxState = (AxHost.State)resources.GetObject("axWindowsMediaPlayer.OcxState");
            axWindowsMediaPlayer.Size = new Size(894, 358);
            axWindowsMediaPlayer.TabIndex = 3;
            axWindowsMediaPlayer.Disconnect += axWindowsMediaPlayer_Disconnect;
            axWindowsMediaPlayer.Buffering += axWindowsMediaPlayer_Buffering;
            axWindowsMediaPlayer.EndOfStream += axWindowsMediaPlayer_EndOfStream;
            axWindowsMediaPlayer.MediaError += axWindowsMediaPlayer_MediaError;
            // 
            // frmStream
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(894, 358);
            Controls.Add(axWindowsMediaPlayer);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "frmStream";
            Text = "Stream";
            WindowState = FormWindowState.Maximized;
            FormClosing += frmStream_FormClosing;
            Load += frmStream_Load;
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer;
    }
}
