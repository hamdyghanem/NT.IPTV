using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace NT.IPTV.Models.Channel
{
    public class SearchTextBox : TextBox
    {
        private System.Windows.Forms.Timer m_delayedTextChangedTimer;
        public int DelayedTextChangedTimeout { get; set; }

        public event EventHandler DelayedTextChanged;

        public SearchTextBox() : base()
        {
            this.DelayedTextChangedTimeout = 2 * 1000; // 2 seconds
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


        protected virtual void OnDelayedTextChanged(EventArgs e)
        {
            if (this.DelayedTextChanged != null)
                this.DelayedTextChanged(this, e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.Cursor= Cursors.WaitCursor;
            this.InitializeDelayedTextChangedEvent();
            base.OnTextChanged(e);
        }

        private void InitializeDelayedTextChangedEvent()
        {
            if (m_delayedTextChangedTimer != null)
                m_delayedTextChangedTimer.Stop();

            if (m_delayedTextChangedTimer == null || m_delayedTextChangedTimer.Interval != this.DelayedTextChangedTimeout)
            {
                m_delayedTextChangedTimer = new System.Windows.Forms.Timer();
                m_delayedTextChangedTimer.Tick += new EventHandler(HandleDelayedTextChangedTimerTick);
                m_delayedTextChangedTimer.Interval = this.DelayedTextChangedTimeout;
            }

            m_delayedTextChangedTimer.Start();
        }

        private void HandleDelayedTextChangedTimerTick(object sender, EventArgs e)
        {
            System.Windows.Forms.Timer timer = sender as System.Windows.Forms.Timer;
            timer.Stop();

            this.OnDelayedTextChanged(EventArgs.Empty);
            this.Cursor= Cursors.Default;
        }
    }

}
