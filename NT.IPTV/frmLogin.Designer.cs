namespace NT.IPTV
{
    partial class frmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            btnGo = new Button();
            tableLayoutPanel1 = new TableLayoutPanel();
            txtUsername = new TextBox();
            lblUserName = new Label();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblProfile = new Label();
            cboProfile = new ComboBox();
            errorProvider1 = new ErrorProvider(components);
            btnSave = new Button();
            statusStrip = new StatusStrip();
            lblVersion = new ToolStripStatusLabel();
            lblStatus = new ToolStripStatusLabel();
            picLogo = new PictureBox();
            btnCancel = new Button();
            chkUseProxy = new CheckBox();
            chkAutoLogin = new CheckBox();
            chkUseBuiltInPlayer = new CheckBox();
            chkDarkMode = new CheckBox();
            txtServer = new TextBox();
            lblServer = new Label();
            lblPort = new Label();
            txtPort = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // btnGo
            // 
            btnGo.Location = new Point(517, 205);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(117, 36);
            btnGo.TabIndex = 0;
            btnGo.Text = "Go";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += btnGo_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.7513237F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 77.24868F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 141F));
            tableLayoutPanel1.Controls.Add(txtUsername, 1, 2);
            tableLayoutPanel1.Controls.Add(lblUserName, 0, 2);
            tableLayoutPanel1.Controls.Add(lblServer, 0, 4);
            tableLayoutPanel1.Controls.Add(lblPassword, 0, 3);
            tableLayoutPanel1.Controls.Add(txtPassword, 1, 3);
            tableLayoutPanel1.Controls.Add(txtServer, 1, 4);
            tableLayoutPanel1.Controls.Add(lblProfile, 0, 0);
            tableLayoutPanel1.Controls.Add(cboProfile, 1, 0);
            tableLayoutPanel1.Controls.Add(lblPort, 0, 5);
            tableLayoutPanel1.Controls.Add(txtPort, 1, 5);
            tableLayoutPanel1.Location = new Point(113, 23);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(659, 176);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(103, 37);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(314, 27);
            txtUsername.TabIndex = 1;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(3, 34);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(75, 20);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "Username";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(3, 67);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(70, 20);
            lblPassword.TabIndex = 0;
            lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(103, 70);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(314, 27);
            txtPassword.TabIndex = 2;
            // 
            // lblProfile
            // 
            lblProfile.AutoSize = true;
            lblProfile.Location = new Point(3, 0);
            lblProfile.Name = "lblProfile";
            lblProfile.Size = new Size(52, 20);
            lblProfile.TabIndex = 0;
            lblProfile.Text = "Profile";
            // 
            // cboProfile
            // 
            cboProfile.FormattingEnabled = true;
            cboProfile.Location = new Point(103, 3);
            cboProfile.Name = "cboProfile";
            cboProfile.Size = new Size(314, 28);
            cboProfile.TabIndex = 4;
            cboProfile.SelectedIndexChanged += cboProfile_SelectedIndexChanged;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(395, 205);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(117, 36);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save Profile";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblVersion, lblStatus });
            statusStrip.Location = new Point(0, 371);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(794, 26);
            statusStrip.TabIndex = 8;
            statusStrip.Text = "statusStrip1";
            // 
            // lblVersion
            // 
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(50, 20);
            lblVersion.Text = "0.0.0.1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(21, 20);
            lblStatus.Text = "....";
            // 
            // picLogo
            // 
            picLogo.Image = Properties.Resources.small_logo;
            picLogo.Location = new Point(12, 23);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(67, 75);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 9;
            picLogo.TabStop = false;
            picLogo.Click += picLogo_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(272, 205);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(117, 36);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // chkUseProxy
            // 
            chkUseProxy.AutoSize = true;
            chkUseProxy.Location = new Point(107, 251);
            chkUseProxy.Margin = new Padding(3, 4, 3, 4);
            chkUseProxy.Name = "chkUseProxy";
            chkUseProxy.Size = new Size(298, 24);
            chkUseProxy.TabIndex = 10;
            chkUseProxy.Text = "Route via Azure Proxy (bypass ISP block)";
            chkUseProxy.UseVisualStyleBackColor = true;
            chkUseProxy.CheckedChanged += chkUseProxy_CheckedChanged;
            // 
            // chkAutoLogin
            // 
            chkAutoLogin.AutoSize = true;
            chkAutoLogin.Location = new Point(107, 279);
            chkAutoLogin.Margin = new Padding(3, 4, 3, 4);
            chkAutoLogin.Name = "chkAutoLogin";
            chkAutoLogin.Size = new Size(174, 24);
            chkAutoLogin.TabIndex = 11;
            chkAutoLogin.Text = "Auto-login on startup";
            chkAutoLogin.UseVisualStyleBackColor = true;
            chkAutoLogin.CheckedChanged += chkAutoLogin_CheckedChanged;
            // 
            // chkUseBuiltInPlayer
            // 
            chkUseBuiltInPlayer.AutoSize = true;
            chkUseBuiltInPlayer.Location = new Point(107, 307);
            chkUseBuiltInPlayer.Margin = new Padding(3, 4, 3, 4);
            chkUseBuiltInPlayer.Name = "chkUseBuiltInPlayer";
            chkUseBuiltInPlayer.Size = new Size(152, 24);
            chkUseBuiltInPlayer.TabIndex = 12;
            chkUseBuiltInPlayer.Text = "Use built-in player";
            chkUseBuiltInPlayer.UseVisualStyleBackColor = true;
            chkUseBuiltInPlayer.CheckedChanged += chkUseBuiltInPlayer_CheckedChanged;
            // 
            // chkDarkMode
            // 
            chkDarkMode.AutoSize = true;
            chkDarkMode.Location = new Point(107, 335);
            chkDarkMode.Margin = new Padding(3, 4, 3, 4);
            chkDarkMode.Name = "chkDarkMode";
            chkDarkMode.Size = new Size(105, 24);
            chkDarkMode.TabIndex = 13;
            chkDarkMode.Text = "Dark mode";
            chkDarkMode.UseVisualStyleBackColor = true;
            chkDarkMode.CheckedChanged += chkDarkMode_CheckedChanged;
            // 
            // txtServer
            // 
            txtServer.Location = new Point(103, 103);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(314, 27);
            txtServer.TabIndex = 3;
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(3, 100);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(50, 20);
            lblServer.TabIndex = 0;
            lblServer.Text = "Server";
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(3, 133);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(35, 20);
            lblPort.TabIndex = 0;
            lblPort.Text = "Port";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(103, 136);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(136, 27);
            txtPort.TabIndex = 3;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(794, 397);
            Controls.Add(picLogo);
            Controls.Add(statusStrip);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            Controls.Add(chkUseProxy);
            Controls.Add(chkAutoLogin);
            Controls.Add(chkUseBuiltInPlayer);
            Controls.Add(chkDarkMode);
            Controls.Add(btnGo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += frmLogin_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnGo;
        private TableLayoutPanel tableLayoutPanel1;
        private TextBox txtUsername;
        private Label lblUserName;
        private Label lblPassword;
        private TextBox txtPassword;
        private ErrorProvider errorProvider1;
        private Label lblProfile;
        private ComboBox cboProfile;
        private Button btnSave;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblVersion;
        private ToolStripStatusLabel lblStatus;
        private PictureBox picLogo;
        private Button btnCancel;
        private CheckBox chkUseProxy;
        private CheckBox chkAutoLogin;
        private CheckBox chkUseBuiltInPlayer;
        private CheckBox chkDarkMode;
        private Label lblServer;
        private TextBox txtServer;
        private Label lblPort;
        private TextBox txtPort;
    }
}
