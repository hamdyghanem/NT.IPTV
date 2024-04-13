using Microsoft.VisualBasic.Devices;
using NT.IPTV.Models.StreamObject;
using NT.IPTV.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace NT.IPTV
{
    public partial class frmDownloader : Form
    {
        private Thread threadStart;
        private WebClient client = new WebClient();
        private string saveDir = Path.Combine(clsCoreOperation.assemblyFolder, clsCoreOperation.DownloadeFolder);
        private IWatch downoadFile;
        private string TitleName;
        List<string> links = new List<string>();
        public frmDownloader()
        {
            InitializeComponent();
        }
        public frmDownloader(IWatch _downoadFile)
        {
            InitializeComponent();
            //
            //
            client = new WebClient();
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            downoadFile = _downoadFile;
            lblInfo.Text = downoadFile.Plot;
            picMovie.ImageLocation = downoadFile.IconUrl;
            var TitleName = cleanName(downoadFile.Name);
            if (downoadFile.Category == enumCategories.Movies)
            {
                var movie = (WatchMovie)downoadFile;
                lblFileName.Text = getFileName(movie.Name, movie.ContainerExtension, 0);
                lblFileName.Tag = movie.StreamUrl;
                this.Text = $"Download: {TitleName}";
                links.Add(movie.StreamUrl);
            }
            else if (downoadFile.Category == enumCategories.Series)
            {
                var series = (WatchSeries)downoadFile;
                var seriesSaveDir = Path.Combine(saveDir, TitleName);
                if (!Directory.Exists(seriesSaveDir))
                    Directory.CreateDirectory(seriesSaveDir);

                foreach (var seasson in series.seasonsData)
                {
                    foreach (var episode in seasson.Episodes)
                    {
                        links.Add(episode.StreamUrl);
                    }
                }
            }
        }
        private async void frmDownloader_Load(object sender, EventArgs e)
        {
            if (downoadFile.Category == enumCategories.Movies)
            {
                //var movie = (WatchMovie)downoadFile;
                startDownload();
            }
            else if (downoadFile.Category == enumCategories.Series)
            {
                var series = (WatchSeries)downoadFile;
                int i = 0;
                foreach (var seasson in series.seasonsData)
                {
                    i++;
                    var seasonPath = Path.Combine(saveDir, TitleName, $"seasons {i}");
                    if (!Directory.Exists(seasonPath))
                        Directory.CreateDirectory(seasonPath);
                    //create folder
                    foreach (var episode in seasson.Episodes)
                    {
                        //set name and file
                        var filePath = Path.Combine(saveDir, TitleName, $"seasons {i}", episode.Name + "." + episode.ContainerExtension);
                        lblFileName.Text = filePath;
                        if (!File.Exists(filePath))
                        {
                            lblFileName.Tag = episode.StreamUrl;
                            //
                            await download();
                        }
                    }
                }
            }
        }
        private void startDownload()
        {
            var filePath = lblFileName.Text;
            var url = lblFileName.Tag.ToString();
            //threadStart = new Thread(() =>
            //  {
            //      client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            //      client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            //      client.DownloadFileAsync(new Uri(url), filePath);
            //  });
            //threadStart.Start();
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            client.DownloadFileAsync(new Uri(url), filePath);
        }
        private async Task download()
        {
            try
            {
                var destinationFilePath = lblFileName.Text;
                var downloadFileUrl = lblFileName.Tag.ToString();

                using (var client = new HttpClientDownloadWithProgress(downloadFileUrl, destinationFilePath))
                {
                    client.ProgressChanged += (totalFileSize, totalBytesDownloaded, progressPercentage) =>
                    {
                        Console.WriteLine($"{progressPercentage}% ({totalBytesDownloaded}/{totalFileSize})");
                    };

                    await client.StartDownload();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
                lblSize.Text = "Downloaded " + e.BytesReceived + " of " + e.TotalBytesToReceive;
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
            lblSize.Text = "Completed";
        });
        }

        public string cleanName(string name)
        {
            return name.Replace(":", " ").Replace("\\", " ").Replace("/", " ");
        }
        private string getFileName(string _name, string _extenstion, int i)
        {
            var file = Path.Combine(saveDir, _name + "." + _extenstion);
            if (i > 0)
            {
                file = Path.Combine(saveDir, _name + "_" + i.ToString() + "." + _extenstion);
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
        public virtual void CleanUp()
        {
            if (client != null)
            {
                client.CancelAsync();
            }
            //
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

        private void lblLinks_Click(object sender, EventArgs e)
        {
            using (frmGetDownloadLinks frm = new frmGetDownloadLinks(String.Join("\r\n", links.ToArray())))
            {
                frm.ShowDialog();
            }
        }
    }
}
