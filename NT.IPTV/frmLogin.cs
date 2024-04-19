using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualBasic.ApplicationServices;
using NT.IPTV.Models;
using NT.IPTV.Utilities;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NT.IPTV
{
    public partial class frmLogin : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private UserInfo _currentUser = new UserInfo();
        private bool logging = false;
        public frmLogin()
        {
            InitializeComponent();
            //
            clsCore.LoadConfiguration();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            lblVersion.Text = fvi.FileVersion;

            clsCore.loadUsersFromDirectory(cboProfile);
            if (!string.IsNullOrEmpty(clsCore.Config.LastProfile))
            {
                cboProfile.Text = clsCore.Config.LastProfile;
                //cboProfile.SelectedIndex = 0;
            }
        }

        private async void btnGo_Click(object sender, EventArgs e)
        {
            if (logging) return;
            logging = true;
            // Access the current MainWindow clsCore
            clsCore.currentUser.Name = cboProfile.Text;
            clsCore.currentUser.UserName = txtUsername.Text;
            clsCore.currentUser.Password = txtPassword.Text;
            clsCore.currentUser.Server = txtServer.Text;
            //clsCore.currentUser.port = txtPort.Text;
            //clsCore.currentUser.useHttps = (bool)protocolCheckBox.IsChecked;

            try
            {
                lblStatus.Text = "Attempting to connect...";
                if (await clsCore.CheckLoginConnection(_cts.Token)) // Connect to the server
                {
                    await clsCore.RetrieveCategories(lblStatus, _cts.Token);
                    await clsCore.RetrieveStreams(_cts.Token);

                    //  //await clsCore.RetrieveStreams(lblStatus, _cts.Token);
                    //  _ = Task.Run(async () =>
                    //{
                    //    await clsCore.RetrieveStreams(_cts.Token);
                    //});

                    if (!_cts.IsCancellationRequested)
                    {
                        this.Hide();
                        frmCategories frm = new frmCategories();
                        frm.Show();
                    }
                }
                else
                {
                    lblStatus.Text = ""; // Clear the busy content if the connection fails
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please ensure all fields are filled out before attempting to connect.");
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
            }
            catch (Exception ex)
            {
                lblStatus.Text = ""; // Clear the busy content
                MessageBox.Show($"Failed to connect. Error: {ex.Message}");
            }
            finally
            {
                _cts = new CancellationTokenSource(); // Reset the token
                logging = false;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
        }

        private void cboProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProfile.SelectedItem == null)
            {
                MessageBox.Show("You must select a user to load");
                return;
            }
            _currentUser = clsCore.GetUserData(cboProfile.SelectedItem.ToString());
            if (_currentUser != null)
            {
                loadDataIntoTextFields();
                // Optional: Clear the selection if needed or keep it based on your UI logic
                // cboProfile.SelectedItem = null;
            }
            clsCore.Config.LastProfile = cboProfile.SelectedItem.ToString();
            clsCore.SaveConfiguration();
        }
        private void loadDataIntoTextFields()
        {
            if (_currentUser?.UserName == null || _currentUser?.Password == null || _currentUser?.Server == null || _currentUser?.Port == null)
            {
                MessageBox.Show("User data is missing, unable to load " + cboProfile.SelectedValue.ToString());
                return;
            }

            txtUsername.Text = _currentUser.UserName;
            txtPassword.Text = _currentUser.Password;
            //protocolCheckBox.IsChecked = _currentUser.UseHttps;
            txtServer.Text = _currentUser.Server;
            txtPort.Text = _currentUser.Port;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username input field is blank");
                return;
            }
            if (string.IsNullOrWhiteSpace(cboProfile.Text))
            {
                MessageBox.Show("Give it a name");
                return;
            }
            _currentUser.Name = cboProfile.Text;
            _currentUser.UserName = txtUsername.Text;
            _currentUser.Password = txtPassword.Text;
            //_currentUser.UseHttps = (bool)protocolCheckBox.IsChecked;
            _currentUser.Server = txtServer.Text;
            _currentUser.Port = txtPort.Text;
            clsCore.loadUsersFromDirectory(cboProfile);
            MessageBox.Show(_currentUser.Name + "'s data saved");
        }

        private void picLogo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "NileTechno.com",
                UseShellExecute = true
            });
        }
    }
}
