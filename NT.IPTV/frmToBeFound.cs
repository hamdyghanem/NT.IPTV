using NT.IPTV.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace NT.IPTV
{
    public partial class frmToBeFound : Form
    {
        private readonly string JsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToBeFound.json");
        private List<ToBeFoundItem> items = new();

        public string SelectedNodeText { get; set; }
        public enumCategories SelectedNodeCategory { get; set; }

        public frmToBeFound()
        {
            InitializeComponent();
        }

        private void frmToBeFound_Load(object sender, EventArgs e)
        {
            LoadTree();
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
                    foreach (var item in items)
                        treeView1.Nodes.Add(new TreeNode(item.ToString()) { Tag = item });
                }
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
                var textBox = new TextBox() { Left = 100, Top = 30, Width = 170, Text = defaultValue };

                // Category label and ComboBox
                var catLabel = new Label() { Left = 10, Top = 75, Text = "Category", Width = 80 };
                var comboBox = new ComboBox() { Left = 100, Top = 70, Width = 170, DropDownStyle = ComboBoxStyle.DropDownList };
                comboBox.DataSource = Enum.GetValues(typeof(enumCategories));
                comboBox.SelectedItem = defaultCategory;

                var okButton = new Button() { Text = "OK", Left = 110, Width = 80, Top = 140, DialogResult = DialogResult.OK };
                inputForm.Controls.Add(promptLabel);
                inputForm.Controls.Add(textLabel);
                inputForm.Controls.Add(textBox);
                inputForm.Controls.Add(catLabel);
                inputForm.Controls.Add(comboBox);
                inputForm.Controls.Add(okButton);
                inputForm.AcceptButton = okButton;

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return (textBox.Text, (enumCategories)comboBox.SelectedItem);
                }
                return ("", defaultCategory);
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null && e.Node.Tag is ToBeFoundItem item)
            {
                SelectedNodeText = item.Text;
                SelectedNodeCategory = item.Category;
                this.DialogResult = DialogResult.OK;
                this.Close();
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