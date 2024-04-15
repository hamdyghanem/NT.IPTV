using NT.IPTV.Models;
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
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NT.IPTV
{
    public partial class FlowCatControl : UserControl
    {
        public List<StreamCategory> Categories { get; set; }
        private RowCatControl lastestRowCatControl;
        private StreamCategory selectedItem;
        public StreamCategory SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
            }
        }

        private int seasonNum { get; set; }
        private string defaultImage { get; set; }

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user clicks button")]
        public event EventHandler SelectedIndexChaged;
        public FlowCatControl()
        {
            InitializeComponent();
        }

        public void LoadCategories(List<StreamCategory> _groups, ToolStripProgressBar progBar)
        {
            Categories = _groups;
            LoadCategories(progBar);
        }
        public void LoadCategories(ToolStripProgressBar progBar)
        {
            flowLayoutPanel.Controls.Clear();
            progBar.Visible = true;
            progBar.Value = 0;
            progBar.Maximum = Categories.Count;
            //
            foreach (var item in Categories)
            {
                progBar.Value++;
                RowCatControl ctrl = new RowCatControl(item, defaultImage);
                ctrl.ButtonClick += new EventHandler(ChannelControl_ButtonClick);
                ctrl.MouseEnter += row_MouseEnter;
                ctrl.MouseLeave += row_MouseLeave;
                flowLayoutPanel.Controls.Add(ctrl);
            }
            progBar.Visible = false;
        }
        protected void ChannelControl_ButtonClick(object sender, EventArgs e)
        {
            if (lastestRowCatControl != null)
            {
                lastestRowCatControl.BackColor = Color.Black;
                lastestRowCatControl.ForeColor = Color.White;
                lastestRowCatControl.Selected = false;
            }
            SelectedItem = ((RowCatControl)sender).Category;
            if (this.SelectedIndexChaged != null)
                this.SelectedIndexChaged(sender, e);
            //
            lastestRowCatControl = (RowCatControl)sender;
            lastestRowCatControl.BackColor = Color.Blue;
            lastestRowCatControl.ForeColor = Color.White;
            lastestRowCatControl.Selected = true;
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
            var ctrl = (RowCatControl)sender;
            if (!ctrl.Selected)
            {
                ctrl.BackColor = Color.AliceBlue;
                ctrl.ForeColor = Color.Black;
            }
        }
        private void row_MouseLeave(object sender, EventArgs e)
        {
            var ctrl = (RowCatControl)sender;
            if (!ctrl.Selected)
            {
                ctrl.BackColor = Color.Black;
                ctrl.ForeColor = Color.White;
            }
        }
    }
}
