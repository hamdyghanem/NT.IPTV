using NT.IPTV.Models.Items;
using NT.IPTV.Models.Items.Channesl;
using NT.IPTV.Utilities;
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
            lblChannelName.Text = Channel?.Name;
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
            lblChannelName.Tag = Channel.Favorite ? "1" : "0";
            if (lblChannelName.Tag == "1")
            {
                lblChannelName.BackColor = Color.Gold;
                lblChannelName.ForeColor = Color.Black;
            }
            this.Size = new Size(clsCore.Config.ThumbnailSize, clsCore.Config.ThumbnailSize);
        }

        private void picLogo_DoubleClick(object sender, EventArgs e)
        {

        }

        private void picLogo_Click(object sender, EventArgs e)
        {
            if (this.ButtonClick != null)
                this.ButtonClick(this, e);
        }

        private void lblChannelName_Click(object sender, EventArgs e)
        {
            //add to favorites
            List<string> lst = new List<string>();
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Live:
                    {
                        lst = clsCore.currentUser.FavoritChannels;
                        clsCore.AllStreamChannels.Single(x => x.ID == Channel.ID).Favorite = lblChannelName.Tag == "0";
                        break;
                    }
                case enumCategories.Movies:
                    {
                        lst = clsCore.currentUser.FavoritMovies;
                        clsCore.AllStreamVideos.Single(x => x.ID == Channel.ID).Favorite = lblChannelName.Tag == "0";
                        break;
                    }
                case enumCategories.Series:
                    {
                        lst = clsCore.currentUser.FavoritSeries;
                        clsCore.AllStreamSerieses.Single(x => x.ID == Channel.ID).Favorite = lblChannelName.Tag == "0";
                        break;
                    }
            }
            //
            if (lblChannelName.Tag == "0")
            {
                lblChannelName.Tag = "1";
                lblChannelName.BackColor = Color.Gold;
                lblChannelName.ForeColor = Color.Black;
                if (!lst.Contains(Channel.ID))
                {
                    lst.Add(Channel.ID);
                }
            }
            else
            {
                lblChannelName.Tag = "0";
                lblChannelName.BackColor = Color.Black;
                lblChannelName.ForeColor = Color.White;
                if (lst.Contains(Channel.ID))
                {
                    lst.Remove(Channel.ID);
                }
            }
            clsCore.SaveConfiguration();
        }
    }
}
