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
using System.Text.Json;
using System.Threading;
using System.Xml.Linq;

namespace NT.IPTV
{
    public partial class frmGlobalSearch : Form
    {
        private readonly string JsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToBeFound.json");
        private List<ToBeFoundItem> items = new();
        public string SelectedNodeText { get; set; }
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public List<IChannel> FoundStreamChannel = new List<IChannel>();
        public frmGlobalSearch()
        {
            InitializeComponent();
        }
        private void frmGlobalSearch_Load(object sender, EventArgs e)
        {
            LoadTree();

            lstGlobalSearch.Items.Clear();
            this.Text = $"Global Search : {clsCore.CurrentCategory}";
            lblFound.Text = "";
            txtSearchMovies.Focus();
            txtSearchMovies.Clear();
        }
        private void LoadTree()
        {
            treeView1.Nodes.Clear();
            items.Clear();
            if (File.Exists(JsonFilePath))
            {
                var loaded = JsonSerializer.Deserialize<List<ToBeFoundItem>>(File.ReadAllText(JsonFilePath));
                if (loaded != null)
                {
                    items.AddRange(loaded);
                    foreach (var item in items.Where(x => x.Category == clsCore.CurrentCategory))
                    {
                        treeView1.Nodes.Add(new TreeNode(item.ToString()) { Tag = item });
                    }
                }
            }
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
        private void DoSearch()
        {
            var filterText = txtSearchMovies.Text.ToLower();
            Cursor = Cursors.WaitCursor;
            lstGlobalSearch.Items.Clear();
            prgBar.Minimum = 0;
            prgBar.Value = 0;
            lblFound.Text = "";
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

        private void txtSearchMovies_DelayedTextChanged(object sender, EventArgs e)
        {
            DoSearch();

        }

        private void lstGlobalSearch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstGlobalSearch_DoubleClick(object sender, EventArgs e)
        {
            btnOk_Click(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var result = Prompt("Enter item:", "", enumCategories.Live);
            if (!string.IsNullOrWhiteSpace(result.text))
            {
                var item = new ToBeFoundItem { Text = result.text, Category = result.category };
                items.Add(item);
                treeView1.Nodes.Add(new TreeNode(item.ToString()) { Tag = item });
                SaveTree();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is ToBeFoundItem item)
            {
                var result = Prompt("Edit item name:", item.Text, item.Category);
                if (!string.IsNullOrWhiteSpace(result.text))
                {
                    item.Text = result.text;
                    item.Category = result.category;
                    treeView1.SelectedNode.Text = item.ToString();
                    SaveTree();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode.Tag is ToBeFoundItem item)
            {
                items.Remove(item);
                treeView1.Nodes.Remove(treeView1.SelectedNode);
                SaveTree();
            }
        }
        private void SaveTree()
        {
            File.WriteAllText(JsonFilePath, JsonSerializer.Serialize(items));
        }

        // Returns (text, category) tuple
        private (string text, enumCategories category) Prompt(string prompt, string defaultValue = "", enumCategories defaultCategory = enumCategories.Live)
        {
            using (var inputForm = new Form())
            {
                inputForm.FormBorderStyle = FormBorderStyle.Fixed3D;
                inputForm.Width = 300;
                inputForm.Height = 220;
                inputForm.StartPosition = FormStartPosition.CenterScreen;

                // Prompt label (top)
                var promptLabel = new Label() { Left = 10, Top = 10, Text = prompt, Width = 260 };

                // Text label (above TextBox)
                var textLabel = new Label() { Left = 10, Top = 35, Text = "Name", Width = 80 };
                var textBox = new System.Windows.Forms.TextBox() { Left = 100, Top = 30, Width = 170, Text = defaultValue };

                var okButton = new System.Windows.Forms.Button() { Text = "OK", Left = 110, Width = 80, Top = 140, DialogResult = DialogResult.OK };
                inputForm.Controls.Add(promptLabel);
                inputForm.Controls.Add(textLabel);
                inputForm.Controls.Add(textBox);
                inputForm.Controls.Add(okButton);
                inputForm.AcceptButton = okButton;

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return (textBox.Text, clsCore.CurrentCategory);
                }
                return ("", defaultCategory);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null && e.Node.Tag is ToBeFoundItem item)
            {
                txtSearchMovies.Text = item.Text;
                DoSearch();
            }
        }
    }
    public class ToBeFoundItem
    {
        public string Text { get; set; }
        public enumCategories Category { get; set; } = enumCategories.Live;
        public override string ToString() => $"{Text} [{Category}]";
    }
}
