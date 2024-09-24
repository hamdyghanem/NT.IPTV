using Microsoft.VisualBasic.Devices;
using NT.IPTV.Models.Channel;
using NT.IPTV.Models.Items;
using NT.IPTV.Models.Items.Channesl;
using NT.IPTV.Models.Items.StreamObject;
using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NT.IPTV
{
    public partial class frmGlobalSearch : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public List<IChannel> FoundStreamChannel = new List<IChannel>();
        public frmGlobalSearch()
        {
            InitializeComponent();
        }
        private void frmGlobalSearch_Load(object sender, EventArgs e)
        {
            txtSearchMovies.Clear();
            lstGlobalSearch.Items.Clear();
            this.Text += $"frmGlobalSearch : {clsCore.CurrentCategory}";
            lblFound.Text = "";
        }

        private void frmGlobalSearch_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        private void lblFileName_Click(object sender, EventArgs e)
        {

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            //remove results
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void txtSearchMovies_TextChanged(object sender, EventArgs e)
        {

        }
        private void txtSearchMovies_DelayedTextChanged(object sender, EventArgs e)
        {
            var filterText = txtSearchMovies.Text.ToLower();
            Cursor = Cursors.WaitCursor;
            lstGlobalSearch.Items.Clear();
            prgBar.Minimum = 0;
            prgBar.Value = 0;
            lblFound.Text = "";
            //
            if (string.IsNullOrEmpty(filterText))
            {
                return;
            }
            List<IChannel> tosearchIn = new List<IChannel>();
            switch (clsCore.CurrentCategory)
            {
                case enumCategories.Live:
                    {
                        tosearchIn = clsCore.AllStreamChannels.ToList<IChannel>();
                        break;
                    }
                case enumCategories.Movies:
                    {
                        tosearchIn = clsCore.AllStreamVideos.ToList<IChannel>();
                        break;
                    }
                case enumCategories.Series:
                    {
                        tosearchIn = clsCore.AllStreamSerieses.ToList<IChannel>();
                        break;
                    }
            }
            FoundStreamChannel = tosearchIn
                    .Where(x => x.Name.ToLower().Contains(filterText)).ToList();
            prgBar.Maximum = FoundStreamChannel.Count;
            lblFound.Text = $"Found:{FoundStreamChannel.Count}";
            foreach (var item in FoundStreamChannel)
            {
                Application.DoEvents();
                prgBar.Value++;
                lstGlobalSearch.Items.Add(item.ToString());
            }
            Cursor = Cursors.Default;
        }

        private void lstGlobalSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstGlobalSearch_DoubleClick(object sender, EventArgs e)
        {
            btnOk_Click(sender, e);
        }
    }
}
