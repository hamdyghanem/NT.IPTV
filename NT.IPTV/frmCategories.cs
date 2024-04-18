using System.Configuration;
using System.Net.Http;
using System.Reflection;
using System.Threading.Channels;
using System.Timers;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NT.IPTV.Utilities;
using System.Collections.Generic;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Diagnostics;
using NT.IPTV.Properties;
using NT.IPTV.Models.Items.Channesl;
using NT.IPTV.Models.Items;

namespace NT.IPTV
{
    public partial class frmCategories : Form
    {
        private System.Timers.Timer _updateCheckTimer;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private List<StreamCategory> categoryies = new List<StreamCategory>();
        private frmGlobalSearch frmGSearch = new frmGlobalSearch();
        private bool bStopProcess = false;
        public frmCategories()
        {
            InitializeComponent();
            loadCategories();//loads the categories into the listbox view
            InitializeUpdateCheckTimer(); // Initialize the update check timer

            //
            btnLive.Tag = enumCategories.Live;
            btnSeries.Tag = enumCategories.Series;
            btnMovies.Tag = enumCategories.Movies;

        }

        private void frmCategories_Load(object sender, EventArgs e)
        {

        }
        #region Auto updating EPG Data Functions
        private void InitializeUpdateCheckTimer()
        {
            // Create and configure the timer
            _updateCheckTimer = new System.Timers.Timer(60000); // Trigger every minute (60000 milliseconds) Maybe make this a config in settings
            _updateCheckTimer.Elapsed += UpdateCheckTimer_Elapsed;
            _updateCheckTimer.AutoReset = true;
            _updateCheckTimer.Start();
        }

        private async void UpdateCheckTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // Check if it's time to update EPG data based on your logic
            if (clsCore.ShouldUpdateOnInterval(DateTime.Now))
            {
                // Perform the update on the UI thread if needed
                await UpdateEpgData();
            }
        }

        private async Task UpdateEpgData()
        {
            _cts = new CancellationTokenSource();
            try
            {
                MessageBox.Show("EPG Updated!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating EPG: {ex.Message}");
            }
            finally
            {
                _cts.Dispose();
            }
        }

        #endregion
        private void loadCategories()
        {
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Live:
                    categoryies = clsCore.ChannelCategories.OrderByDescending(n => n.Favorite).ToList();
                    break;
                case enumCategories.Movies:
                    categoryies = clsCore.MoviesCategories.OrderByDescending(n => n.Favorite).ToList();
                    break;
                case enumCategories.Series:
                    categoryies = clsCore.SeriesCategories.OrderByDescending(n => n.Favorite).ToList();
                    break;
            }
            FillCategoryList(categoryies);
        }

        private void FillCategoryList(List<StreamCategory> groups)
        {
            lblStatus.Text = string.Empty;
            flwCat.LoadCategories(groups, prgBar);
            //select first one
            flwCat.SelectByIndex(0); ;
        }

        private void backToLoginBtn_Click(object sender, EventArgs e)
        {
            /*UserLogin.ReturnToLogin = true;
            this.Close();*/
        }
        private async void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = this.flwCat.SelectedItem;
            await clsCore.RetrieveStreams(selectedItem, _cts.Token);
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Live:
                    {
                        FillSubCategories(clsCore.StreamChannels);
                    }
                    break;
                case enumCategories.Movies:
                    {
                        FillSubCategories(clsCore.StreamVideos);
                    }
                    break;
                case enumCategories.Series:
                    {
                        FillSubCategories(clsCore.StreamSerieses);
                    }
                    break;
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (_cts != null)
            {
                _cts.Cancel();
            }
        }
        private void txtSearch_DelayedTextChanged(object sender, EventArgs e)
        {
            bStopProcess = true;
            flwChannel.Controls.Clear();
            //
            var filterText = txtSearch.Text.ToLower();
            var filteredItems = categoryies
                .Where(channel => channel.Name.ToLower().Contains(filterText)).ToList();
            FillCategoryList(filteredItems);

        }

        private void txtSearchMovies_DelayedTextChanged(object sender, EventArgs e)
        {
            var filterText = txtSearchMovies.Text.ToLower();
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Live:
                    {
                        var filteredItems = clsCore.StreamChannels
                     .Where(channel => channel.Name.ToLower().Contains(filterText)).ToList();
                        FillSubCategories(filteredItems);
                    }
                    break;
                case enumCategories.Movies:
                    {
                        var filteredItems = clsCore.StreamVideos
                     .Where(channel => channel.Name.ToLower().Contains(filterText)).ToList();
                        FillSubCategories(filteredItems);
                    }
                    break;
                case enumCategories.Series:
                    {
                        var filteredItems = clsCore.StreamSerieses
                     .Where(channel => channel.Name.ToLower().Contains(filterText)).ToList();
                        FillSubCategories(filteredItems);
                    }
                    break;
            }
        }
        private void FillSubCategories<T>(List<T> channels)
        {
            flwChannel.Controls.Clear();
            Cursor = Cursors.WaitCursor;
            foreach (var ch in channels)
            {
                Application.DoEvents();
                var ctrl = new ChannelControl((IChannel)ch);
                ctrl.ButtonClick += new EventHandler(ChannelControl_ButtonClick);
                flwChannel.Controls.Add(ctrl);
            }
            Cursor = Cursors.Default;
        }

        protected void ChannelControl_ButtonClick(object sender, EventArgs e)
        {
            try
            {
                //handle the event 
                var ctrl = (ChannelControl)sender;
                if (ctrl.Channel.Category == enumCategories.Live)
                {
                    //frmStream frm = new frmStream(ctrl.Channel.StreamUrl);
                    //frm.ShowDialog();
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

                        if (!string.IsNullOrEmpty(ctrl.Channel.StreamUrl))
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo
                            {
                                FileName = "cmd.exe",
                                Arguments = $"/C \"{vlcLocatedPath}\" {ctrl.Channel.StreamUrl}",
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
                else
                {
                    frmMovieData frm = new frmMovieData((ChannelControl)sender);
                    frm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmCategories_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsCore.SaveConfiguration();
            Application.ExitThread();
        }

        private async void btnLive_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            ToggleButtons(btn);
            clsCore.CurrentCategory = (enumCategories)btn.Tag;
            loadCategories();
        }
        private async void btnGlobalSearch_Click(object sender, EventArgs e)
        {
            if (clsCore.CurrentCategory == enumCategories.Live && clsCore.AllStreamChannels.Count == 0 ||
                clsCore.CurrentCategory == enumCategories.Movies && clsCore.AllStreamVideos.Count == 0 ||
                clsCore.CurrentCategory == enumCategories.Series && clsCore.AllStreamSerieses.Count == 0)
            {
                MessageBox.Show("Still loading data, please wait and try again.");
                await clsCore.RetrieveStreams(_cts.Token);
                return;
            }
            if (frmGSearch.ShowDialog() == DialogResult.OK)
            {
                FillSubCategories(frmGSearch.FoundStreamChannel);
            }
        }
        private void ToggleButtons(ToolStripButton btn)
        {
            btnLive.Checked = false;
            btnSeries.Checked = false;
            btnMovies.Checked = false;
            btn.Checked = true;
            bStopProcess = true;
            flwChannel.Controls.Clear();
        }

        private void btnBigger_Click(object sender, EventArgs e)
        {
            if (flwChannel.Controls.Count > 0)
            {
                var iinterval = 1.2;
                var newSize = new Size(Convert.ToInt32(flwChannel.Controls[0].Size.Width * iinterval), Convert.ToInt32(flwChannel.Controls[0].Size.Width * iinterval));
                if (((Button)sender).Tag.ToString() == "/")
                {
                    newSize = new Size(Convert.ToInt32(flwChannel.Controls[0].Size.Width / iinterval), Convert.ToInt32(flwChannel.Controls[0].Size.Width / iinterval));
                }
                clsCore.Config.ThumbnailSize = flwChannel.Controls[0].Size.Width;
                foreach (Control ctrl in flwChannel.Controls)
                {
                    if (bStopProcess)
                    {
                        bStopProcess = false;
                        break;
                    }
                    Application.DoEvents();
                    ctrl.Size = newSize;
                }
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            System.Drawing.Bitmap img;
            //
            IEnumerable<ChannelControl> lst = flwChannel.Controls.OfType<ChannelControl>();
            List<ChannelControl> ordered = new List<ChannelControl>();
            if (button.Tag == "down")
            {
                ordered = lst.OrderBy(x => x.Channel.Name).ToList();
                img = Properties.Resources.NameUp;
            }
            else
            {
                ordered = lst.OrderByDescending(x => x.Channel.Name).ToList();
                img = Properties.Resources.NameDown;
            }

            fillOrderedControl(ordered, button, img);
        }

        private void btnDateUp_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            System.Drawing.Bitmap img;
            //
            IEnumerable<ChannelControl> lst = flwChannel.Controls.OfType<ChannelControl>();
            List<ChannelControl> ordered = new List<ChannelControl>();
            if (button.Tag == "down")
            {
                ordered = lst.OrderBy(x => x.Channel.ReleaseDate).ToList();
                img = Properties.Resources.YearUp;
            }
            else
            {
                ordered = lst.OrderByDescending(x => x.Channel.ReleaseDate).ToList();
                img = Properties.Resources.YearDown;
            }

            fillOrderedControl(ordered, button, img);
        }

        private void btnRatingUp_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            System.Drawing.Bitmap img;
            //
            IEnumerable<ChannelControl> lst = flwChannel.Controls.OfType<ChannelControl>();
            List<ChannelControl> ordered = new List<ChannelControl>();
            if (button.Tag == "down")
            {
                ordered = lst.OrderBy(x => x.Channel.Rating).ToList();
                img = Properties.Resources.RatingUp;
            }
            else
            {
                ordered = lst.OrderByDescending(x => x.Channel.Rating).ToList();
                img = Properties.Resources.RatingDown;
            }

            fillOrderedControl(ordered, button, img);
        }
        private void fillOrderedControl(List<ChannelControl> ordered, Button btn, System.Drawing.Bitmap img)
        {
            if (btn.Tag == "down")
            {
                btn.Tag = "up";
            }
            else
            {
                btn.Tag = "down";
            }
            btn.BackgroundImage = img;
            //
            flwChannel.Controls.Clear();
            foreach (Control ctrl in ordered)
            {
                ctrl.Size = new Size(clsCore.Config.ThumbnailSize, clsCore.Config.ThumbnailSize);
                flwChannel.Controls.Add(ctrl);
            }
        }

        private void txtSearchMovies_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void flwCat_Load(object sender, EventArgs e)
        {

        }
    }
}
