using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using Microsoft.VisualBasic.Devices;
using NT.IPTV.Models.Items.StreamObject;
using NT.IPTV.Utilities;

namespace NT.IPTV
{
    public partial class frmMovieData : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        ChannelControl cltrParent;
        IWatch Selected;
        List<string> links = new List<string>();

        public frmMovieData(ChannelControl cltr)
        {
            InitializeComponent();
            cltrParent = cltr;
            lblInfo.Text = string.Empty;
            lblCast.Text = string.Empty;
            lblData.Text = string.Empty;
        }


        private async void frmMovieData_Load(object sender, EventArgs e)
        {
            if (cltrParent.Channel.Category == enumCategories.Movies)
            {
                tabSeries.Hide();
                picMovie.Dock = DockStyle.Fill;
                //Load movie Data
                await getMovieInfo();
            }
            else if (cltrParent.Channel.Category == enumCategories.Series)
            {
                picMovie.Dock = DockStyle.Right;
                tabSeries.TabPages.Clear();
                tabSeries.Dock = DockStyle.Fill;
                //Load movie Data
                await getSeriesInfo();
            }
        }

        private async Task<WatchMovie> getMovieInfo()
        {
            Selected = await clsCore.GetMovieInfo(cltrParent.Channel.StreamID, _cts.Token);
            await fillLabels();
            picCover.Click += picMovie_Click;
            links.Add(Selected.StreamUrl);
            return (WatchMovie)Selected;
        }

        private async Task<WatchSeries> getSeriesInfo()
        {
            Selected = await clsCore.GetSeriesInfo(cltrParent.Channel.StreamID, _cts.Token);
            if (Selected != null)
            {
                await fillLabels();

                var series = (WatchSeries)Selected;
                for (int i = 0; i < series.Seasons.Count; i++)
                {
                    var tab = new TabPage($"Season {i + 1}");
                    flowSeriesControl flow = new flowSeriesControl(series.Seasons[i], i, series.SeriesInfo.Cover);
                    flow.ButtonClick += new EventHandler(SeriesControl_ButtonClick);
                    flow.Dock = DockStyle.Fill;
                    tab.BackColor = Color.Black;
                    tab.Controls.Add(flow);
                    //
                    tabSeries.TabPages.Add(tab);
                    //
                    foreach (var episode in series.Seasons[i].Episodes)
                    {
                        links.Add(episode.StreamUrl);
                    }
                }
                return series;
            }
            return null;
        }
        private async Task fillLabels()
        {
            if (Selected == null)
            {
                return;
            }
            lblInfo.Text = Selected.Plot;
            lblCast.Text = Selected.Cast;
            if (!string.IsNullOrEmpty(Selected.Duration))
            {
                lblData.Text = $"Duration: {Selected.Duration} \t ";
            }
            lblData.Text += $"Director: {Selected.Director} \t Genre: {Selected.Genre}";
            if (Selected.BackdropPath.Length == 0)
            {
                picMovie.BackdropPath = new string[] { Selected.IconUrl };
            }
            else
            {
                //ckeck first image
                if (!await clsCore.Check404(Selected.BackdropPath[0]))
                {
                    picMovie.BackdropPath = new string[] { Selected.IconUrl };
                }
                else
                {
                    picMovie.BackdropPath = Selected.BackdropPath;
                }
            }


            if (!string.IsNullOrEmpty(Selected.IconUrl))
            {
                picCover.ImageLocation = Selected.IconUrl;
            }
        }
        protected void SeriesControl_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                //handle the event 
                var ctrl = (RowSeriesControl)sender;
                frmPlayMovie frm = new frmPlayMovie(ctrl.EpisodeData.StreamUrl);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnOpenVLC_Click(object sender, EventArgs e)
        {
            try
            {
                string vlcLocatedPath = clsCore.GetVLCPath(); // Use the dedicated method to get or find the VLC path

                if (string.IsNullOrEmpty(vlcLocatedPath) || !File.Exists(vlcLocatedPath))
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog
                    {
                        InitialDirectory = "c:\\",
                        Filter = "VLC Executable File (*.exe)|*.exe",
                        RestoreDirectory = true
                    };

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        vlcLocatedPath = openFileDialog.FileName;
                        // Optionally, update the configuration with the newly selected path
                        clsCore.Config.VlcLocationPath = vlcLocatedPath;
                        clsCore.SaveConfiguration();
                    }
                    else
                    {
                        MessageBox.Show("VLC path selection was canceled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (!string.IsNullOrEmpty(cltrParent.Channel.StreamUrl))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/C \"{vlcLocatedPath}\" {Selected.StreamUrl}",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show("Stream URL not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open VLC: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




        private void frmMovieData_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void picMovie_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Selected.StreamUrl))
            {
                this.Close();
                frmPlayMovie frm = new frmPlayMovie(Selected.StreamUrl);
                frm.ShowDialog();
                frm.Dispose();
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            using (frmDownloader frm = new frmDownloader(Selected))
            {
                frm.ShowDialog();
            }
        }


        private void btnWatchTrailer_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Selected.YoutubeTrailer))
                {
                    frmPlayMovie frm = new frmPlayMovie($"https://www.youtube.com/watch?v={Selected.YoutubeTrailer}");
                    frm.ShowDialog();
                }
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void btnDownloadLinks_Click(object sender, EventArgs e)
        {
            using (frmGetDownloadLinks frm = new frmGetDownloadLinks(String.Join("\r\n", links.ToArray())))
            {
                frm.ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
