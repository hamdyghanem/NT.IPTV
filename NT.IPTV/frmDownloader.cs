using NT.IPTV.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System.Threading;
using System.Xml.Linq;

namespace NT.IPTV
{
    public partial class frmDownloader : Form
    {
        string url;
        string filePath;
        Thread threadStart;
        WebClient client = new WebClient();
        public frmDownloader()
        {
            InitializeComponent();
        }

        public frmDownloader(string _url, string _name, string _extenstion, string _plot, string _pic)
        {
            InitializeComponent();
            //
            //
            client = new WebClient();
            string saveDir = Path.Combine(clsCoreOperation.assemblyFolder, clsCoreOperation.DownloadeFolder);
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);

            //
            this.url = _url;
            this.Text = $"Download: {_name}";
            _name = _name.Replace(":", " ").Replace("\\", " ").Replace("/", " ");
            filePath = getFileName(_name, _extenstion, 0);
            lblFileName.Text = filePath;
            //

            lblInfo.Text = _plot;
            picMovie.ImageLocation = _pic;

        }
        private string getFileName(string _name, string _extenstion, int i)
        {
            var file = Path.Combine(clsCoreOperation.assemblyFolder, clsCoreOperation.DownloadeFolder, _name + "." + _extenstion);
            if (i > 0)
            {
                file = Path.Combine(clsCoreOperation.assemblyFolder, clsCoreOperation.DownloadeFolder, _name + "_" + i.ToString() + "." + _extenstion);
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
        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void startDownload()
        {
            threadStart = new Thread(() =>
          {
              client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
              client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
              client.DownloadFileAsync(new Uri(url), filePath);
          });
            threadStart.Start();
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
                progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
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

        private void frmDownloader_Load(object sender, EventArgs e)
        {
            startDownload();
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
            string directoryPath = Path.GetDirectoryName(filePath);
            try
            {
                Process.Start("explorer.exe", directoryPath);

            }
            catch { }
        }
    }
}
