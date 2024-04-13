using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Channels;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using NT.IPTV.Models;
using NT.IPTV.Models.StreamObject;
using NT.IPTV.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace NT.IPTV
{
    public partial class frmPlayMovie : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        string StreamUrl;
        public frmPlayMovie(string streamURL)
        {
            InitializeComponent();
            StreamUrl = streamURL;
        }

        private async void frmStreamMovie_Load(object sender, EventArgs e)
        {
            //
            //StringBuilder str = new StringBuilder();
            //str.AppendLine("<head>");
            //str.AppendLine("  <link href='https://vjs.zencdn.net/8.10.0/video-js.css' rel='stylesheet' />");
            //str.AppendLine("</head>");
            //str.AppendLine("<body>");
            //str.AppendLine("  <video");
            //str.AppendLine("    id='my-video'");
            //str.AppendLine("    class='video-js vjs-default-skin' data-setup='{\"fluid\": true}'");
            //str.AppendLine("    controls");
            //str.AppendLine("    preload='auto'");
            //str.AppendLine("    width='640'");
            //str.AppendLine("    height='264'");
            //str.AppendLine("  >");
            //str.AppendLine($"    <source src='{StreamUrl}' type='video/mp4' />");
            //str.AppendLine("    <p class='vjs-no-js'>");
            //str.AppendLine("      To view this video please enable JavaScript, and consider upgrading to a web browser that");
            //str.AppendLine("      <a href='https://videojs.com/html5-video-support/' target='_blank'>supports HTML5 video</a>");
            //str.AppendLine("    </p>");
            //str.AppendLine("  </video>");
            //str.AppendLine("  <script src='https://vjs.zencdn.net/8.10.0/video.min.js'></script>");
            //str.AppendLine("</body>");

            //await webView.EnsureCoreWebView2Async();
            //webView.NavigateToString(str.ToString());
            await webView.EnsureCoreWebView2Async();
            webView.Source = new Uri(StreamUrl);
        }

        private void frmStreamMovie_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
