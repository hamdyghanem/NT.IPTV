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

            // Update visual state based on IsHidden property
            UpdateHiddenStateVisuals();
        }


        private void btnFavorite_Click(object sender, EventArgs e)
        {
            List<string> lst = new List<string>();
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Live:
                    {
                        lst = clsCore.currentUser.FavoritChannelsCategory;
                        clsCore.ChannelCategories.Single(x => x.ID == Category.ID).Favorite = btnFavorite.Tag == "0";
                        break;
                    }
                case enumCategories.Movies:
                    {
                        lst = clsCore.currentUser.FavoritMoviesCategory;
                        clsCore.MoviesCategories.Single(x => x.ID == Category.ID).Favorite = btnFavorite.Tag == "0";
                        break;
                    }
                case enumCategories.Series:
                    {
                        lst = clsCore.currentUser   .FavoritSeriesCategory;
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
            // Toggle the hidden state of the category
            Category.IsHidden = !Category.IsHidden;

            // Update the visual appearance to reflect hidden state
            UpdateHiddenStateVisuals();

            // List to track hidden categories
            List<string> hiddenList = new List<string>();

            // Update the category in the appropriate list based on current category type
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Live:
                    {
                        var cat = clsCore.ChannelCategories.SingleOrDefault(x => x.ID == Category.ID);
                        if (cat != null)
                            cat.IsHidden = Category.IsHidden;
                        hiddenList = clsCore.currentUser.HiddenChannelsCategory;
                        break;
                    }
                case enumCategories.Movies:
                    {
                        var cat = clsCore.MoviesCategories.SingleOrDefault(x => x.ID == Category.ID);
                        if (cat != null)
                            cat.IsHidden = Category.IsHidden;
                        hiddenList = clsCore.currentUser.HiddenMoviesCategory;
                        break;
                    }
                case enumCategories.Series:
                    {
                        var cat = clsCore.SeriesCategories.SingleOrDefault(x => x.ID == Category.ID);
                        if (cat != null)
                            cat.IsHidden = Category.IsHidden;
                        hiddenList = clsCore.currentUser.HiddenSeriesCategory;
                        break;
                    }
            }

            // Add or remove from hidden list
            if (Category.IsHidden)
            {
                if (!hiddenList.Contains(Category.ID))
                {
                    hiddenList.Add(Category.ID);
                }
            }
            else
            {
                if (hiddenList.Contains(Category.ID))
                {
                    hiddenList.Remove(Category.ID);
                }
            }

            // Save the updated configuration
            clsCore.SaveConfiguration();
        }

        private void UpdateHiddenStateVisuals()
        {
            if (Category.IsHidden)
            {
                // Visual indication that this category is hidden - RED background for hidden eye
                btnSettings.BackColor = Color.Red;
                btnSettings.BackgroundImage = null;
                lblName.ForeColor = Color.Gray;
            }
            else
            {
                // Visual indication that this category is visible - BLACK background for visible eye
                btnSettings.BackColor = Color.Black;
                btnSettings.BackgroundImage = null;
                lblName.ForeColor = Color.White;
            }
            btnSettings.Invalidate(); // Redraw the button
        }

        private void BtnSettings_Paint(object sender, PaintEventArgs e)
        {
            // Draw eye icon
            e.Graphics.Clear(btnSettings.BackColor);

            // Eye icon dimensions
            int x = btnSettings.Width / 2 - 8;
            int y = btnSettings.Height / 2 - 5;

            if (Category.IsHidden)
            {
                // Draw a closed/hidden eye icon (slash through eye)
                using (Pen pen = new Pen(Color.White, 2))
                {
                    // Eye outline
                    e.Graphics.DrawEllipse(pen, x, y, 16, 10);
                    // Slash to indicate hidden
                    e.Graphics.DrawLine(pen, x, y + 10, x + 16, y);
                }
            }
            else
            {
                // Draw an open eye icon
                using (Pen pen = new Pen(Color.White, 2))
                {
                    // Eye outline (ellipse)
                    e.Graphics.DrawEllipse(pen, x, y, 16, 10);
                }

                // Draw pupil (small circle)
                using (Brush brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillEllipse(brush, x + 6, y + 3, 4, 4);
                }
            }
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
