//using System;
//using System.Configuration;
//using System.Diagnostics;
//using System.Reflection;
//using System.Security.Policy;
//using System.Threading.Channels;
//using AxWMPLib;
//using Microsoft.VisualBasic.ApplicationServices;
//using NT.IPTV.Models;
//using NT.IPTV.Models.StreamObject;
//using NT.IPTV.Utilities;
//using WMPLib;

//namespace NT.IPTV
//{
//    public partial class frmStream : Form
//    {
//        private CancellationTokenSource _cts = new CancellationTokenSource();
//        string StreamUrl;
//        public frmStream(string streamURL)
//        {
//            InitializeComponent();
//            StreamUrl = streamURL;
//        }

//        private void frmStream_Load(object sender, EventArgs e)
//        {
//            axWindowsMediaPlayer.currentPlaylist.clear();

//            if (!string.IsNullOrEmpty(StreamUrl))
//            {
//                axWindowsMediaPlayer.URL = StreamUrl;
//                axWindowsMediaPlayer.Ctlcontrols.play();
//            }
//        }
//        private void axWindowsMediaPlayer_Disconnect(object sender, AxWMPLib._WMPOCXEvents_DisconnectEvent e)
//        {

//        }

//        private void axWindowsMediaPlayer_Buffering(object sender, AxWMPLib._WMPOCXEvents_BufferingEvent e)
//        {

//        }

//        private void axWindowsMediaPlayer_EndOfStream(object sender, _WMPOCXEvents_EndOfStreamEvent e)
//        {

//        }

//        private void axWindowsMediaPlayer_MediaError(object sender, _WMPOCXEvents_MediaErrorEvent e)
//        {
//            try
//            // If the Player encounters a corrupt or missing file, 
//            // show the hexadecimal error code and URL.
//            {
//                IWMPMedia2 errSource = e.pMediaObject as IWMPMedia2;
//                IWMPErrorItem errorItem = errSource.Error;
//                MessageBox.Show("Error " + errorItem.errorCode.ToString("X")
//                                + " in " + errSource.sourceURL);
//            }
//            catch (InvalidCastException)
//            // In case pMediaObject is not an IWMPMedia item.
//            {
//                MessageBox.Show("Error.");
//            }
//        }
//        private void frmStream_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            axWindowsMediaPlayer.Ctlcontrols.stop();
//            axWindowsMediaPlayer.currentPlaylist.clear();

//        }
//    }
//}
