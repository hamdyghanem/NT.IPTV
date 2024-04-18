using NT.IPTV.Models.Channel;
using NT.IPTV.Models.Items;
using NT.IPTV.Properties;
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
    public partial class RowCatControl : UserControl
    {
        public StreamCategory Category { get; set; }
        public bool Selected { get; set; }
        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks button")]
        public event EventHandler ButtonClick;
        public RowCatControl()
        {
            InitializeComponent();
        }

        public RowCatControl(StreamCategory _category, string defaultImage)
        {
            InitializeComponent();
            //
            Category = _category;
            lblName.Text = Category.Name;
            //lblPlot.Text = Category.Info.plot;
            //lblDuration.Text = Category.Info.Duration;
            //if (!string.IsNullOrEmpty(Category.Info.movie_image))
            //{
            //    picLogo.ImageLocation = Category.Info.movie_image;
            //}
            //else
            //{
            //    picLogo.ImageLocation = defaultImage;
            //}

            btnFavorite.Tag = Category.Favorite ? "1" : "0";
            if (btnFavorite.Tag == "1")
            {
                btnFavorite.BackgroundImage = Properties.Resources.RatingUp;
            }
            else
            {
                btnFavorite.BackgroundImage = Properties.Resources.RatingDown;
            }
        }


        private void btnFavorite_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Live:
                    {
                        lst = clsCore.Config.FavoritChannelsCategory;
                        clsCore.ChannelCategories.Single(x => x.ID == Category.ID).Favorite = btnFavorite.Tag == "0";
                        break;
                    }
                case enumCategories.Movies:
                    {
                        lst = clsCore.Config.FavoritMoviesCategory;
                        clsCore.MoviesCategories.Single(x => x.ID == Category.ID).Favorite = btnFavorite.Tag == "0";
                        break;
                    }
                case enumCategories.Series:
                    {
                        lst = clsCore.Config.FavoritSeriesCategory;
                        clsCore.SeriesCategories.Single(x => x.ID == Category.ID).Favorite = btnFavorite.Tag == "0";
                        break;
                    }
            }
            //
            if (btnFavorite.Tag == "0")
            {
                btnFavorite.Tag = "1";
                btnFavorite.BackgroundImage = Properties.Resources.RatingUp;
                if (!lst.Contains(Category.ID))
                {
                    lst.Add(Category.ID);
                }
            }
            else
            {
                btnFavorite.Tag = "0";
                btnFavorite.BackgroundImage = Properties.Resources.RatingDown;
                if (lst.Contains(Category.ID))
                {
                    lst.Remove(Category.ID);
                }
            }
            clsCore.SaveConfiguration();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

        }

        private void lblName_Click(object sender, EventArgs e)
        {
            if (this.ButtonClick != null)
                this.ButtonClick(this, e);
        }

        private void lblName_MouseEnter(object sender, EventArgs e)
        {
            OnMouseEnter(e);
        }

        private void lblName_MouseLeave(object sender, EventArgs e)
        {
            OnMouseLeave(e);
        }

        private void RowCatControl_ForeColorChanged(object sender, EventArgs e)
        {
            lblName.ForeColor = ForeColor;
        }
    }
}
