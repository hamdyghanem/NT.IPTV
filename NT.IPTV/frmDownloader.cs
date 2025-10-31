using Microsoft.VisualBasic.Devices;
using NT.IPTV.Models.Items.StreamObject;
using NT.IPTV.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NT.IPTV
{
    public partial class frmDownloader : Form
    {
        private Thread threadStart;
        private IWatch downoadFile;
        private string TitleName;
        List<string> links = new List<string>();
        List<string> SeassonsToDownload = new List<string>();
        private bool bClosing;
        public frmDownloader()
        {
            InitializeComponent();
        }
        public frmDownloader(IWatch _downoadFile, List<string> _SeassonsToDownload)
        {
            InitializeComponent();
            //
            SeassonsToDownload = _SeassonsToDownload;
            lstLog.Items.Clear();
            prgBar.Value = 0;
            prgBarSeries.Value = 0;


            downoadFile = _downoadFile;
            picMovie.ImageLocation = downoadFile.IconUrl;
            TitleName = clsCore.CleanName(downoadFile.Name);
            if (downoadFile.Category == enumCategories.Movies)
            {
                var movie = (WatchMovie)downoadFile;
                lblFileName.Text = getFileName(CleanFileName(movie.Name), movie.ContainerExtension, 0);
                MyToolTip.Show(lblFileName.Text, lblFileName);
                lblFileName.Tag = movie.StreamUrl;
                this.Text = $"Download: {TitleName}";
                links.Add(movie.StreamUrl);
            }
            else if (downoadFile.Category == enumCategories.Series)
            {

                var series = (WatchSeries)downoadFile;
                var seriesSaveDir = Path.Combine(clsCore.DownloadeFolder, TitleName);
                if (!Directory.Exists(seriesSaveDir))
                {
                    Directory.CreateDirectory(seriesSaveDir);
                }
                foreach (var seasson in series.Seasons)
                {
                    if (SeassonsToDownload.Contains(seasson.SeasonNum.ToString()))
                    {
                        foreach (var episode in seasson.Episodes)
                        {
                            links.Add(episode.StreamUrl);
                        }
                    }
                }
                lblOverallProgress.Visible = true;
                prgBarSeries.Visible = true;
                prgBarSeries.Maximum = links.Count;
            }
        }
        private async void frmDownloader_Load(object sender, EventArgs e)
        {
            if (downoadFile.Category == enumCategories.Movies)
            {
                await download();
            }
            else if (downoadFile.Category == enumCategories.Series)
            {
                var series = (WatchSeries)downoadFile;
                foreach (var seasson in series.Seasons)
                {
                    if (SeassonsToDownload.Contains(seasson.SeasonNum.ToString()))
                    {
                        var seasonPath = Path.Combine(clsCore.DownloadeFolder, TitleName, $"seasons {seasson.SeasonNum}");
                        if (!Directory.Exists(seasonPath))
                        {
                            Directory.CreateDirectory(seasonPath);
                        }
                        //create folder
                        foreach (var episode in seasson.Episodes)
                        {
                            if (bClosing)
                            {
                                return;
                            }
                            //set name and file
                            var filePath = Path.Combine(clsCore.DownloadeFolder, TitleName, $"seasons {seasson.SeasonNum}", episode.Name + "." + episode.ContainerExtension);
                            lblFileName.Text = filePath;
                            MyToolTip.Show(lblFileName.Text, lblFileName);
                            prgBarSeries.Value++;
                            if (!File.Exists(filePath))
                            {
                                lblFileName.Tag = episode.StreamUrl;
                                await download();
                            }
                        }
                    }
                }
            }
        }
        private async Task download()
        {
            try
            {
                var destinationFilePath = lblFileName.Text;
                var downloadFileUrl = lblFileName.Tag.ToString();
                lstLog.Items.Add("Start downloading ...");
                lstLog.Items.Add(downloadFileUrl);

                using (var client = new HttpClientDownloadWithProgress(downloadFileUrl, destinationFilePath))
                {
                    client.ProgressChanged += (totalFileSize, totalBytesDownloaded, progressPercentage) =>
                    {
                        if (bClosing)
                        {
                            //cancel will delete the file
                            client.Cancel();
                            return;
                        }
                        lblPercentage.Text = $"{progressPercentage} % {ConvertBytesToString(totalBytesDownloaded)} of {ConvertBytesToString(totalFileSize ?? 0)}";
                        prgBar.Value = int.Parse(Math.Truncate(progressPercentage ?? 0).ToString());
                    };

                    await client.StartDownload();
                }
                lblPercentage.Text = "Completed";
                lstLog.Items.Add("Completed");
                this.Close();
            }
            catch (Exception ex)
            {
                lstLog.Items.Add(ex.Message);
            }
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            try
            {
                this.BeginInvoke((MethodInvoker)delegate
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                lblPercentage.Text = $"{string.Format("{0:0.00}", percentage)} %";
                prgBar.Value = int.Parse(Math.Truncate(percentage).ToString());
            });
            }
            catch { }
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.CleanUp(); // Method that disposes the client and unhooks events
                return;
            }
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }

            this.BeginInvoke((MethodInvoker)delegate
        {
            lblPercentage.Text = "100%";
        });
        }
        private string getFileName(string _name, string _extenstion, int i)
        {
            var file = Path.Combine(clsCore.DownloadeFolder, _name + "." + _extenstion);
            if (i > 0)
            {
                file = Path.Combine(clsCore.DownloadeFolder, _name + "_" + i.ToString() + "." + _extenstion);
            }
            if (File.Exists(file))
            {
                return getFileName(_name, _extenstion, i + 1);
            }
            else
            {
                return file;
            }
        }
        private string CleanFileName(string _name)
        {
            _name = _name.Replace(":", "_");
            return _name;
        }
        public virtual void CleanUp()
        {
            bClosing = true;
            if (threadStart != null)
            {
                threadStart.Interrupt();
            }
        }
        private void frmDownloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            CleanUp();
        }

        private void lblFileName_Click(object sender, EventArgs e)
        {
            string directoryPath = Path.GetDirectoryName(lblFileName.Text);
            try
            {
                Process.Start("explorer.exe", directoryPath);

            }
            catch { }
        }

        static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
        static string ConvertBytesToString(long bytes)
        {
            if ((bytes / 1024f) / 1024f > 1024)
            {
                return ((bytes / 1024f) / 1024f / 1024f).ToString("0.00") + "TB";
            }
            else
            {
                return ((bytes / 1024f) / 1024f).ToString("0.00") + "MB";
            }
        }
    }
}
