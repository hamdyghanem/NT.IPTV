using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using Microsoft.VisualBasic.ApplicationServices;
using NT.IPTV.Models;
using NT.IPTV.Utilities;

namespace NT.IPTV
{
    /// <summary>
    /// Login form for NT.IPTV application.
    /// Provides user authentication, profile management, and application configuration.
    /// </summary>
    public partial class frmLogin : Form
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private bool logging = false;
        /// <summary>
        /// Initializes the login form, loads configuration, and applies the saved theme.
        /// </summary>
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
                // Load the profile data into text fields
                // (Setting .Text doesn't trigger SelectedIndexChanged, so we do it manually)
                clsCore.currentUser = clsCore.GetUserData(clsCore.Config.LastProfile);
                if (clsCore.currentUser != null)
                {
                    txtUsername.Text = clsCore.currentUser.UserName ?? string.Empty;
                    txtPassword.Text = clsCore.currentUser.Password ?? string.Empty;
                    txtServer.Text = clsCore.currentUser.Server ?? string.Empty;
                    txtPort.Text = clsCore.currentUser.Port ?? string.Empty;
                }
            }

            // Restore proxy toggle
            chkUseProxy.Checked = clsCore.Config.UseAzureProxy;
            chkAutoLogin.Checked = clsCore.Config.AutoLogin;
            chkUseBuiltInPlayer.Checked = clsCore.Config.UseBuiltInPlayer;
            chkDarkMode.Checked = clsCore.Config.DarkMode;

            // Apply saved theme immediately so the login window already looks themed
            clsCore.ApplyTheme(this);
        }

        /// <summary>
        /// Handles form load event. Sets up keyboard shortcuts, tooltips, and initiates auto-login if configured.
        /// </summary>
        private void frmLogin_Load(object sender, EventArgs e)
        {
            // Restore form position and size if saved previously
            if (clsCore.Config.LoginFormX >= 0 && clsCore.Config.LoginFormY >= 0)
            {
                this.Location = new Point(clsCore.Config.LoginFormX, clsCore.Config.LoginFormY);
                this.Size = new Size(clsCore.Config.LoginFormWidth, clsCore.Config.LoginFormHeight);
            }

            // If the user holds the Shift key down on startup, bypass auto-login
            if (Control.ModifierKeys == Keys.Shift)
            {
                lblStatus.Text = "⏭️ Auto-login bypassed (Shift held)";
                return;
            }

            // Set up keyboard shortcuts
            this.KeyPreview = true;
            this.KeyDown += FrmLogin_KeyDown;

            // Set up tooltips for better accessibility
            SetupTooltips();

            // Hook into FormClosing to save state
            this.FormClosing += FrmLogin_FormClosing;

            // Auto-login: if enabled and profile data is present, connect immediately
            if (clsCore.Config.AutoLogin && !string.IsNullOrEmpty(clsCore.Config.LastProfile))
            {
                btnGo_Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Saves form state when closing (position, size, etc).
        /// </summary>
        private void FrmLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save form position and size
            if (this.WindowState == FormWindowState.Normal)
            {
                clsCore.Config.LoginFormX = this.Location.X;
                clsCore.Config.LoginFormY = this.Location.Y;
                clsCore.Config.LoginFormWidth = this.Width;
                clsCore.Config.LoginFormHeight = this.Height;
                clsCore.SaveConfiguration();
            }
        }

        /// <summary>
        /// Handles keyboard shortcuts for the login form.
        /// </summary>
        private void FrmLogin_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter key: Connect
            if (e.KeyCode == Keys.Enter && !btnGo.Focused)
            {
                btnGo_Click(this, EventArgs.Empty);
                e.Handled = true;
            }

            // Escape key: Cancel
            if (e.KeyCode == Keys.Escape)
            {
                btnCancel_Click(this, EventArgs.Empty);
                e.Handled = true;
            }

            // Ctrl+S: Save Profile
            if (e.KeyCode == Keys.S && e.Control)
            {
                btnSave_Click(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Sets up helpful tooltips for form controls.
        /// </summary>
        private void SetupTooltips()
        {
            var toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 500;
            toolTip.ReshowDelay = 500;

            toolTip.SetToolTip(cboProfile, "Select an existing profile or type a name for a new profile\nUse keyboard: Tab to navigate");
            toolTip.SetToolTip(txtUsername, "Enter your username (min 3 characters)\nShortcut: Tab to move to next field");
            toolTip.SetToolTip(txtPassword, "Enter your password (min 4 characters)\nClick 👁 button or press it to show password");
            toolTip.SetToolTip(txtServer, "Enter server address (hostname or IP)\nExample: example.com or 192.168.1.1");
            toolTip.SetToolTip(txtPort, "Enter port number (1-65535)\nDefault IPTV ports: 80, 8080, 8000");
            toolTip.SetToolTip(btnGo, "Connect to the IPTV server\nShortcut: Enter key");
            toolTip.SetToolTip(btnSave, "Save the current profile for future login\nShortcut: Ctrl+S");
            toolTip.SetToolTip(btnCancel, "Cancel the current connection attempt\nShortcut: Escape key");
        }

        /// <summary>
        /// Handles the Connect button click event.
        /// Validates input, attempts connection to the IPTV server, loads catalog, and opens the main window.
        /// </summary>
        private async void btnGo_Click(object sender, EventArgs e)
        {
            if (logging) return;

            // Pre-submit validation
            var validation = ValidationHelper.ValidateAllLoginFields(
                txtUsername.Text,
                txtPassword.Text,
                txtServer.Text,
                txtPort.Text,
                cboProfile.Text
            );

            if (!validation.IsValid)
            {
                UserMessageHelper.ShowValidationError(validation.ErrorMessage, "Input Validation");
                return;
            }

            logging = true;
            SetLoginFormState(false); // Disable form controls during login

            // Access the current MainWindow clsCore
            clsCore.currentUser.Name = cboProfile.Text;
            clsCore.currentUser.UserName = txtUsername.Text;
            clsCore.currentUser.Password = txtPassword.Text;
            clsCore.currentUser.Server = txtServer.Text;
            //clsCore.currentUser.port = txtPort.Text;
            //clsCore.currentUser.useHttps = (bool)protocolCheckBox.IsChecked;

            try
            {
                lblStatus.Text = "🔄 Attempting to connect...";
                if (await clsCore.CheckLoginConnection(_cts.Token)) // Connect to the server
                {
                    // Try loading catalog from local cache first for instant startup
                    if (clsCore.TryLoadCatalogCache())
                    {
                        var age = DateTime.UtcNow - clsCore.CatalogTimestamp;
                        lblStatus.Text = $"📦 Loaded from cache ({(int)age.TotalHours}h {age.Minutes}m ago). Tap Refresh to update.";
                    }
                    else
                    {
                        // Fresh fetch from server
                        clsCore.CatalogLoadedFromCache = false;
                        lblStatus.Text = "⬇️ Downloading catalog...";
                        await clsCore.RetrieveCategories(lblStatus, _cts.Token);
                        await clsCore.RetrieveStreams(_cts.Token);
                        // Persist to cache for next launch
                        clsCore.SaveCatalogCache();
                        lblStatus.Text = "✅ Catalog loaded successfully!";
                    }

                    if (!_cts.IsCancellationRequested)
                    {
                        System.Threading.Thread.Sleep(500); // Brief pause to show success message
                        this.Hide();
                        frmCategories frm = new frmCategories();
                        frm.Show();
                    }
                }
                else
                {
                    lblStatus.Text = "❌ Connection failed. Please check your credentials and server details.";
                }
            }
            catch (ArgumentNullException ex)
            {
                UserMessageHelper.LogException(ex, "Login - ArgumentNull");
                UserMessageHelper.ShowValidationError("An unexpected error occurred. Please ensure all fields are properly filled.", "Validation Error");
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation
                UserMessageHelper.LogDebug("Login attempt was cancelled by user");
                lblStatus.Text = "⏸️ Connection cancelled.";
            }
            catch (Exception ex)
            {
                lblStatus.Text = ""; // Clear the busy content
                UserMessageHelper.LogException(ex, "Login - Connection");
                UserMessageHelper.ShowConnectionError(ex);
            }
            finally
            {
                _cts = new CancellationTokenSource(); // Reset the token
                logging = false;
                SetLoginFormState(true); // Re-enable form controls after login attempt
            }
        }

        /// <summary>
        /// Enables or disables login form input controls based on connection state.
        /// </summary>
        private void SetLoginFormState(bool enabled)
        {
            // Disable/enable input fields
            cboProfile.Enabled = enabled;
            txtUsername.Enabled = enabled;
            txtPassword.Enabled = enabled;
            txtServer.Enabled = enabled;
            txtPort.Enabled = enabled;
            btnShowPassword.Enabled = enabled;

            // Disable/enable buttons
            btnGo.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = true; // Cancel should always be enabled

            // Show/hide loading indicator
            if (!enabled)
            {
                lblStatus.Text = "Processing...";
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
            }
            else
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }
        /// <summary>
        /// Cancels the current connection attempt by signaling the cancellation token.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            _cts.Cancel();
        }

        private void chkUseProxy_CheckedChanged(object sender, EventArgs e)
        {
            clsCore.Config.UseAzureProxy = chkUseProxy.Checked;
            clsCore.SaveConfiguration();
        }

        private void chkAutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            clsCore.Config.AutoLogin = chkAutoLogin.Checked;
            clsCore.SaveConfiguration();
        }

        private void chkUseBuiltInPlayer_CheckedChanged(object sender, EventArgs e)
        {
            clsCore.Config.UseBuiltInPlayer = chkUseBuiltInPlayer.Checked;
            clsCore.SaveConfiguration();
        }

        private void chkDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            clsCore.Config.DarkMode = chkDarkMode.Checked;
            clsCore.SaveConfiguration();
            // Re-skin the login form live
            clsCore.ApplyTheme(this);
        }

        /// <summary>
        /// Handles profile dropdown selection change. Loads the selected profile's data into the form fields.
        /// </summary>
        private void cboProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboProfile.SelectedItem == null || string.IsNullOrEmpty(cboProfile.Text))
            {
                // Clear fields if no profile is selected
                ClearTextFields();
                return;
            }

            clsCore.currentUser = clsCore.GetUserData(cboProfile.SelectedItem.ToString());

            if (clsCore.currentUser != null)
            {
                loadDataIntoTextFields();
                UserMessageHelper.LogDebug($"Profile '{cboProfile.SelectedItem}' loaded successfully.");
            }
            else
            {
                UserMessageHelper.ShowInfo("Profile not found or could not be loaded.", "Profile Load");
                ClearTextFields();
                return;
            }

            clsCore.Config.LastProfile = cboProfile.SelectedItem.ToString();
            clsCore.SaveConfiguration();
        }

        /// <summary>
        /// Clears all input text fields.
        /// </summary>
        private void ClearTextFields()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtServer.Clear();
            txtPort.Clear();
            lblPasswordStrength.Text = string.Empty;
        }
        private void loadDataIntoTextFields()
        {
            if (clsCore.currentUser?.UserName == null || clsCore.currentUser?.Password == null || clsCore.currentUser?.Server == null || clsCore.currentUser?.Port == null)
            {
                UserMessageHelper.ShowInfo($"Profile '{cboProfile.SelectedValue}' has incomplete data. Please fill in all fields and save again.", "Incomplete Profile");
                ClearTextFields();
                return;
            }

            txtUsername.Text = clsCore.currentUser.UserName;
            txtPassword.Text = clsCore.currentUser.Password;
            //protocolCheckBox.IsChecked = clsCore.currentUser.UseHttps;
            txtServer.Text = clsCore.currentUser.Server;
            txtPort.Text = clsCore.currentUser.Port;
        }

        /// <summary>
        /// Saves the current profile data to persistent storage.
        /// Validates all fields before saving.
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate all fields before saving
            var validation = ValidationHelper.ValidateAllLoginFields(
                txtUsername.Text,
                txtPassword.Text,
                txtServer.Text,
                txtPort.Text,
                cboProfile.Text
            );

            if (!validation.IsValid)
            {
                UserMessageHelper.ShowValidationError(validation.ErrorMessage, "Profile Save Validation");
                return;
            }

            clsCore.currentUser.Name = cboProfile.Text;
            clsCore.currentUser.UserName = txtUsername.Text;
            clsCore.currentUser.Password = txtPassword.Text;
            //clsCore.currentUser.UseHttps = (bool)protocolCheckBox.IsChecked;
            clsCore.currentUser.Server = txtServer.Text;
            clsCore.currentUser.Port = txtPort.Text;
            clsCore.SaveConfiguration();
            clsCore.loadUsersFromDirectory(cboProfile);
            UserMessageHelper.ShowSuccess($"Profile '{clsCore.currentUser.Name}' data saved successfully.", "Profile Saved");
        }

        private void picLogo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "NileTechno.com",
                UseShellExecute = true
            });
        }

        /// <summary>
        /// Shows password when mouse button is held down on the show password button.
        /// </summary>
        private void btnShowPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = false;
        }

        /// <summary>
        /// Hides password when mouse button is released from the show password button.
        /// </summary>
        private void btnShowPassword_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
        }

        /// <summary>
        /// Updates password strength indicator as the user types in the password field.
        /// </summary>
        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            string password = txtPassword.Text;
            if (string.IsNullOrEmpty(password))
            {
                lblPasswordStrength.Text = string.Empty;
                return;
            }

            int strength = ValidationHelper.CalculatePasswordStrength(password);
            string strengthDesc = ValidationHelper.GetPasswordStrengthDescription(strength);

            lblPasswordStrength.Text = $"Password strength: {strengthDesc}";

            // Color code the strength indicator
            lblPasswordStrength.ForeColor = strength switch
            {
                < 30 => Color.Red,
                < 60 => Color.Orange,
                < 80 => Color.YellowGreen,
                _ => Color.Green
            };
        }
    }
}
