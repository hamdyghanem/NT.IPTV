using LibVLCSharp.Shared;
using NT.IPTV.Utilities;

namespace NT.IPTV
{
    public partial class frmPlayMovie : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;
        private string StreamUrl;

        // UI Controls
        private Panel pnlControls;
        private Button btnPlayPause;
        private TrackBar tbProgress;
        private TrackBar tbVolume;
        private Label lblTime;
        private Button btnPiP;
        private System.Windows.Forms.Timer tmrUI;

        public frmPlayMovie(string streamURL)
        {
            InitializeComponent();
            StreamUrl = streamURL;
            Core.Initialize();
            InitializePlayerUI();
        }

        private void InitializePlayerUI()
        {
            pnlControls = new Panel { Dock = DockStyle.Bottom, Height = 50, BackColor = Color.FromArgb(40, 40, 40) };
            btnPlayPause = new Button { Text = "Pause", Width = 80, Dock = DockStyle.Left, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnPlayPause.Click += BtnPlayPause_Click;

            tbVolume = new TrackBar { Dock = DockStyle.Right, Width = 100, Minimum = 0, Maximum = 100, Value = 100, TickStyle = TickStyle.None };
            tbVolume.Scroll += TbVolume_Scroll;

            btnPiP = new Button { Text = "PiP", Width = 50, Dock = DockStyle.Right, FlatStyle = FlatStyle.Flat, ForeColor = Color.White };
            btnPiP.Click += BtnPiP_Click;

            lblTime = new Label { Text = "00:00 / 00:00", Width = 100, Dock = DockStyle.Right, ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter };

            tbProgress = new TrackBar { Dock = DockStyle.Fill, Minimum = 0, Maximum = 1000, TickStyle = TickStyle.None };
            tbProgress.Scroll += TbProgress_Scroll;

            pnlControls.Controls.Add(tbProgress);
            pnlControls.Controls.Add(lblTime);
            pnlControls.Controls.Add(btnPiP);
            pnlControls.Controls.Add(tbVolume);
            pnlControls.Controls.Add(btnPlayPause);
            
            this.Controls.Add(pnlControls);
            pnlControls.BringToFront();

            tmrUI = new System.Windows.Forms.Timer { Interval = 1000 };
            tmrUI.Tick += TmrUI_Tick;

            // Toggle controls on video click
            videoView.MouseClick += (s, e) => { pnlControls.Visible = !pnlControls.Visible; };
        }

        private void frmStreamMovie_Load(object sender, EventArgs e)
        {
            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);
            videoView.MediaPlayer = _mediaPlayer;

            _mediaPlayer.TimeChanged += (s, ev) => { }; // Keep alive
            _mediaPlayer.EndReached += (s, ev) => { this.Invoke((MethodInvoker)delegate { this.Close(); }); };

            // Load and play the media
            using var media = new Media(_libVLC, new Uri(clsCore.GetProxiedUrl(StreamUrl)));

            if (clsCore.Config.PlaybackPositions.TryGetValue(StreamUrl, out long savedTimeMs) && savedTimeMs > 0)
            {
                media.AddOption($":start-time={(savedTimeMs / 1000.0).ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            }

            _mediaPlayer.Play(media);
            tmrUI.Start();
        }

        private void frmStreamMovie_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_mediaPlayer != null)
            {
                long currentPos = _mediaPlayer.Time;
                long length = _mediaPlayer.Length;

                if (currentPos > 0 && length > 0)
                {
                    // Only save if we watched less than 95% to avoid resuming at the very end
                    if (currentPos < length * 0.95)
                        clsCore.Config.PlaybackPositions[StreamUrl] = currentPos;
                    else
                        clsCore.Config.PlaybackPositions.Remove(StreamUrl);

                    clsCore.SaveConfiguration();
                }
            }

            tmrUI?.Stop();
            _mediaPlayer?.Stop();
            _mediaPlayer?.Dispose();
            _libVLC?.Dispose();
        }

        private void TmrUI_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer == null || !_mediaPlayer.IsPlaying) return;

            long time = _mediaPlayer.Time;
            long length = _mediaPlayer.Length;

            if (length > 0)
            {
                lblTime.Text = $"{TimeSpan.FromMilliseconds(time):hh\\:mm\\:ss} / {TimeSpan.FromMilliseconds(length):hh\\:mm\\:ss}";
                if (!tbProgress.Focused) // Don't update if user is dragging
                {
                    tbProgress.Value = (int)((time * 1000) / length);
                }
            }
            else
            {
                lblTime.Text = "Live";
                tbProgress.Visible = false;
            }
        }

        private void BtnPlayPause_Click(object sender, EventArgs e)
        {
            if (_mediaPlayer == null) return;
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
                btnPlayPause.Text = "Play";
            }
            else
            {
                _mediaPlayer.Play();
                btnPlayPause.Text = "Pause";
            }
        }

        private void TbVolume_Scroll(object sender, EventArgs e)
        {
            if (_mediaPlayer != null)
                _mediaPlayer.Volume = tbVolume.Value;
        }

        private void TbProgress_Scroll(object sender, EventArgs e)
        {
            if (_mediaPlayer != null && _mediaPlayer.Length > 0)
            {
                long newTime = (_mediaPlayer.Length * tbProgress.Value) / 1000;
                _mediaPlayer.Time = newTime;
            }
        }

        private bool isPiP = false;
        private void BtnPiP_Click(object sender, EventArgs e)
        {
            isPiP = !isPiP;
            if (isPiP)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal;
                this.Size = new Size(400, 225);
                this.TopMost = true;
                this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width - 20, Screen.PrimaryScreen.WorkingArea.Height - this.Height - 20);
                pnlControls.Visible = false; // Hide controls in PiP
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
                this.WindowState = FormWindowState.Maximized;
                pnlControls.Visible = true;
            }
        }
    }
}
