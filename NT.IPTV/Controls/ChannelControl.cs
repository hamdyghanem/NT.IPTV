using NT.IPTV.Models.Channel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NT.IPTV
{
    public partial class ChannelControl : UserControl
    {
        public IChannel Channel { get; set; }
        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks button")]
        public event EventHandler ButtonClick;
        public ChannelControl()
        {
            InitializeComponent();
        }

        public ChannelControl(IChannel _channel)
        {
            InitializeComponent();
            //
            Channel = _channel;
            lblChannelName.Text = Channel?.DisplayName;
            //lblDesc.Text = channel.Title + channel.Description;
            //System.Net.WebRequest request = System.Net.WebRequest.Create(channel.LogoUrl);
            //System.Net.WebResponse response = request.GetResponse();
            //System.IO.Stream responseSwtream =
            //    response.GetResponseStream();
            //picLogo.BackgroundImage = new Bitmap(responseStream);
            if (!string.IsNullOrEmpty(Channel?.IconUrl))
            {
                picLogo.ImageLocation = Channel.IconUrl;
            }
        }

        private void picLogo_DoubleClick(object sender, EventArgs e)
        {

        }

        private void picLogo_Click(object sender, EventArgs e)
        {
            if (this.ButtonClick != null)
                this.ButtonClick(this, e);
        }
    }
}
