using NT.IPTV.Models.Channel;
using NT.IPTV.Models.StreamObject;
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
    public partial class RowSeriesControl : UserControl
    {
        public EpisodeData EpisodeData { get; set; }
        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks button")]
        public event EventHandler ButtonClick;
        public RowSeriesControl()
        {
            InitializeComponent();
        }

        public RowSeriesControl(EpisodeData _episodeData, string defaultImage)
        {
            InitializeComponent();
            //
            EpisodeData = _episodeData;
            lblName.Text = EpisodeData.Name;
            lblPlot.Text = EpisodeData.Info.plot;
            lblDuration.Text = EpisodeData.Info.Duration;
            if (!string.IsNullOrEmpty(EpisodeData.Info.movie_image))
            {
                picLogo.ImageLocation = EpisodeData.Info.movie_image;
            }
            else
            {
                picLogo.ImageLocation = defaultImage;

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

        private void lblDuration_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);

        }

        private void lblDuration_Click(object sender, EventArgs e)
        {

        }

        private void lblDuration_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);

        }
    }
}
