using System.Configuration;
using System.Net.Http;
using System.Reflection;
using System.Threading.Channels;
using System.Timers;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NT.IPTV.Models;
using NT.IPTV.Utilities;
using NT.IPTV.Models.Channel;
using System.Collections.Generic;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Diagnostics;

namespace NT.IPTV
{
    public partial class frmCategories : Form
    {
        private System.Timers.Timer _updateCheckTimer;
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private List<StreamCategory> categoryies = new List<StreamCategory>();
        private bool bStopProcess =false;
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
            categoryies = clsCore.ChannelCategories.OrderBy(x => x.Name).ToList();
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Movies:
                    categoryies = clsCore.MoviesCategories.OrderBy(x => x.Name).ToList();
                    break;
                case enumCategories.Series:
                    categoryies = clsCore.SeriesCategories.OrderBy(x => x.Name).ToList();
                    break;
            }
            FillCategoryList(categoryies);
        }

        private void FillCategoryList(List<StreamCategory> groups)
        {
            lstCategories.Items.Clear();
            txtSearchMovies.Clear();
            lblStatus.Text = "loading...";
            prgBar.Value = 0;
            prgBar.Maximum = groups.Count;
            foreach (var item in groups)
            {
                lstCategories.Items.Add(item);
                prgBar.Value++;
            }
            lblStatus.Text = string.Empty;
        }
        
        private void backToLoginBtn_Click(object sender, EventArgs e)
        {
            /*UserLogin.ReturnToLogin = true;
            this.Close();*/
        }
        private void lstCategories_SelectedValueChanged(object sender, EventArgs e)
        {

        }
        private async void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstCategories.SelectedItems.Count == 0)
                return;

            var selectedItem = (StreamCategory)this.lstCategories.SelectedItems[0];
            await clsCoreOperation.RetrieveStreams(selectedItem, _cts.Token);
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
            Application.ExitThread();
        }

        private async void btnLive_Click(object sender, EventArgs e)
        {
            var btn = (ToolStripButton)sender;
            ToggleButtons(btn);
            clsCore.CurrentCategory = (enumCategories)btn.Tag;
            await clsCoreOperation.RetrieveCategories(_cts.Token);
            loadCategories();
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
                foreach (Control ctrl in flwChannel.Controls)
                {
                    if (bStopProcess)
                    {
                        bStopProcess=false;
                        break;
                    }
                    Application.DoEvents();
                    ctrl.Size = newSize;
                }
            }
        }
    }
}
