using System;
using System.Configuration;
using System.Diagnostics;
using System.Text;
using NT.IPTV.Models.StreamObject;
using NT.IPTV.Utilities;

namespace NT.IPTV
{
    public partial class frmMovieData : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        ChannelControl cltrParent;
        IMovie Selected;
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

        private async Task getMovieInfo()
        {
            Selected = await clsCoreOperation.GetMovieInfo(cltrParent.Channel.StreamID, _cts.Token);
            fillLabels();
            picCover.Click += picMovie_Click;
            var movie = (Movie)Selected;
        }

        private async Task getSeriesInfo()
        {
            Selected = await clsCoreOperation.GetSeriesInfo(cltrParent.Channel.StreamID, _cts.Token);
            fillLabels();

            var series = (Series)Selected;
            for (int i = 0; i < series.seasonsData.Count; i++)
            {
                var tab = new TabPage($"Season {i + 1}");
                flowSeriesControl flow = new flowSeriesControl(series.seasonsData[i], i, series.SeriesInfo.Cover);
                flow.ButtonClick += new EventHandler(SeriesControl_ButtonClick);
                flow.Dock = DockStyle.Fill;
                tab.BackColor = Color.Black;
                tab.Controls.Add(flow);
                //
                tabSeries.TabPages.Add(tab);
            }
        }
        private void fillLabels()
        {
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
                picMovie.BackdropPath = Selected.BackdropPath;
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
                string vlcLocatedPath = ConfigManager.GetVLCPath(); // Use the dedicated method to get or find the VLC path

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
                        ConfigManager.UpdateSetting("vlcLocationPath", vlcLocatedPath);
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
            this.Close();
            frmPlayMovie frm = new frmPlayMovie(Selected.StreamUrl);
            frm.ShowDialog();
            frm.Dispose();
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (cltrParent.Channel.Category == enumCategories.Movies)
            {
                using (frmDownloader frm = new frmDownloader(Selected.StreamUrl, Selected.Name, Selected.ContainerExtension, Selected.Plot, Selected.IconUrl))
                {
                    frm.ShowDialog();
                }
            }
            else if (cltrParent.Channel.Category == enumCategories.Series)
            {
                MessageBox.Show("Sorry we still working on this feature, but you still can have this links and download them");
                Series series = (Series)Selected;
                StringBuilder links = new StringBuilder();     
                foreach (var seasson in series.seasonsData)
                {
                    foreach (var episode in seasson.Episodes)
                    {
                        links.AppendLine(episode.StreamUrl);
                    }
                }
                using (frmGetDownloadLinks frm = new frmGetDownloadLinks(links.ToString()))
                {
                    frm.ShowDialog();
                }
            }
        }

        private void btnDownloadViaWeb_Click(object sender, EventArgs e)
        {
            try
            {
                if (cltrParent.Channel.Category == enumCategories.Movies)
                {
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = Selected.StreamUrl,
                        UseShellExecute = true
                    });


                }
                else if (cltrParent.Channel.Category == enumCategories.Series)
                {

                }
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
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
    }
}
