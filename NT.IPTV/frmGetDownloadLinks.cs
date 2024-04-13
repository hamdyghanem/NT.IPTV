using System.Configuration;
using System.Reflection;
using Microsoft.VisualBasic.ApplicationServices;
using NT.IPTV.Models;
using NT.IPTV.Utilities;

namespace NT.IPTV
{
    public partial class frmGetDownloadLinks : Form
    {
        private string links = string.Empty;
        public frmGetDownloadLinks()
        {
            InitializeComponent();

        }
        public frmGetDownloadLinks(string _links)
        {
            InitializeComponent();
            links = links;
            txtLinks.Text = _links;   
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
