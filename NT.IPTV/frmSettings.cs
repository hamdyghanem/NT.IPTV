using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NT.IPTV.Utilities;
using Newtonsoft.Json;

namespace NT.IPTV
{
    public partial class frmSettings : Form
    {
        private string downloadFolder;

        public frmSettings()
        {
            InitializeComponent();
            LoadSettings();
            ApplyTheme();
        }

        private void LoadSettings()
        {
            try
            {
                // Load download folder from configuration
                downloadFolder = clsCore.Config.DownloadFolder;
                if (string.IsNullOrEmpty(downloadFolder))
                {
                    // Default to the configured download folder or use fallback
                    downloadFolder = clsCore.GetDownloadFolder();
                }
                txtDownloadFolder.Text = downloadFolder;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ApplyTheme()
        {
            clsCore.ApplyTheme(this);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description = "Select Download Folder";
                folderDialog.SelectedPath = downloadFolder;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    downloadFolder = folderDialog.SelectedPath;
                    txtDownloadFolder.Text = downloadFolder;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate that folder exists
                if (!Directory.Exists(downloadFolder))
                {
                    MessageBox.Show("Selected folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Save to configuration
                clsCore.Config.DownloadFolder = downloadFolder;
                clsCore.SaveConfiguration();

                MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
