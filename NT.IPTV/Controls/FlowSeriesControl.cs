using NT.IPTV.Models.Channel;
using NT.IPTV.Models.Items.StreamObject;
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
    public partial class flowSeriesControl : UserControl
    {
        private Season season { get; set; }
        private int seasonNum { get; set; }
        private string defaultImage { get; set; }

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks button")]
        public event EventHandler ButtonClick;
        public flowSeriesControl()
        {
            InitializeComponent();
        }

        public flowSeriesControl(Season _season, int _seasonNum, string _defaultImage)
        {
            InitializeComponent();
            season = _season;
            seasonNum = _seasonNum;
            defaultImage = _defaultImage;
            LoadSeasons();
        }

        private void LoadSeasons()
        {
            foreach (var item in season.Episodes)
            {
                RowSeriesControl ctrl = new RowSeriesControl(item, defaultImage);
                ctrl.ButtonClick += new EventHandler(ChannelControl_ButtonClick);
                if (!item.IsDownloaded)
                {
                    ctrl.MouseEnter += row_MouseEnter;
                    ctrl.MouseLeave += row_MouseLeave;
                }
                if (item.IsDownloaded)
                {
                    ctrl.BackColor = Color.DarkSeaGreen;
                }
                flowLayoutPanel.Controls.Add(ctrl);
            }
        }
        protected void ChannelControl_ButtonClick(object sender, EventArgs e)
        {
            if (this.ButtonClick != null)
                this.ButtonClick(sender, e);
        }


        private void flowLayoutPanel_SizeChanged(object sender, EventArgs e)
        {
            flowLayoutPanel.SuspendLayout();
            foreach (Control ctrl in flowLayoutPanel.Controls)
            {
                Application.DoEvents();
                ctrl.Width = flowLayoutPanel.ClientSize.Width;

            }
            flowLayoutPanel.ResumeLayout();

        }
        private void row_MouseEnter(object sender, EventArgs e)
        {
            var ctrl = (RowSeriesControl)sender;
            ctrl.BackColor = Color.LightSkyBlue;

        }
        private void row_MouseLeave(object sender, EventArgs e)
        {
            var ctrl = (RowSeriesControl)sender;
            ctrl.BackColor = Color.Black;
        }
    }
}
