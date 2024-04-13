namespace NT.IPTV
{
    using System;
    using System.Windows.Forms;

    public class PicBackdropPath : PictureBox
    {
        public int DelayedTextChangedTimeout { get; set; }

        private int currentImage = 0;
        private string[] backdropPath;
        public string[] BackdropPath
        {
            get
            {
                return this.backdropPath;
            }
            set
            {
                this.backdropPath = value;
                LoadImages();
            }
        }

        private System.Windows.Forms.Timer m_delayedTextChangedTimer;
        public PicBackdropPath() : base()
        {
            this.DelayedTextChangedTimeout = 4 * 1000; // 4 seconds
        }

        private void LoadImages()
        {
            if (BackdropPath!=null && BackdropPath.Length > 0)
            {
                m_delayedTextChangedTimer = new System.Windows.Forms.Timer();
                m_delayedTextChangedTimer.Tick += new EventHandler(HandleDelayedTextChangedTimerTick);
                m_delayedTextChangedTimer.Interval = this.DelayedTextChangedTimeout;
                m_delayedTextChangedTimer.Start();
            }
        }
        private void HandleDelayedTextChangedTimerTick(object sender, EventArgs e)
        {
            this.ImageLocation = BackdropPath[currentImage];
            currentImage++;
            if (currentImage == BackdropPath.Length)
            {
                currentImage = 0;
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (m_delayedTextChangedTimer != null)
            {
                m_delayedTextChangedTimer.Stop();
                if (disposing)
                    m_delayedTextChangedTimer.Dispose();
            }

            base.Dispose(disposing);
        }

    }

}
