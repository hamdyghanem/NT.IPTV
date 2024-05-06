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
            lblServer = new Label();
            lblPassword = new Label();
            txtPassword = new TextBox();
            txtServer = new TextBox();
            lblProfile = new Label();
            cboProfile = new ComboBox();
            lblPort = new Label();
            txtPort = new TextBox();
            errorProvider1 = new ErrorProvider(components);
            btnSave = new Button();
            statusStrip = new StatusStrip();
            lblVersion = new ToolStripStatusLabel();
            lblStatus = new ToolStripStatusLabel();
            picLogo = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
            SuspendLayout();
            // 
            // btnGo
            // 
            btnGo.Location = new Point(458, 193);
            btnGo.Margin = new Padding(3, 2, 3, 2);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(102, 27);
            btnGo.TabIndex = 0;
            btnGo.Text = "Go";
            btnGo.UseVisualStyleBackColor = true;
            btnGo.Click += btnGo_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 22.7513237F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 77.24868F));
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
            tableLayoutPanel1.Location = new Point(99, 17);
            tableLayoutPanel1.Margin = new Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(496, 161);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(115, 29);
            txtUsername.Margin = new Padding(3, 2, 3, 2);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(275, 23);
            txtUsername.TabIndex = 1;
            // 
            // lblUserName
            // 
            lblUserName.AutoSize = true;
            lblUserName.Location = new Point(3, 27);
            lblUserName.Name = "lblUserName";
            lblUserName.Size = new Size(60, 15);
            lblUserName.TabIndex = 0;
            lblUserName.Text = "Username";
            // 
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(3, 81);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(39, 15);
            lblServer.TabIndex = 0;
            lblServer.Text = "Server";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(3, 54);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(57, 15);
            lblPassword.TabIndex = 0;
            lblPassword.Text = "Password";
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(115, 56);
            txtPassword.Margin = new Padding(3, 2, 3, 2);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(275, 23);
            txtPassword.TabIndex = 2;
            // 
            // txtServer
            // 
            txtServer.Location = new Point(115, 83);
            txtServer.Margin = new Padding(3, 2, 3, 2);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(275, 23);
            txtServer.TabIndex = 3;
            // 
            // lblProfile
            // 
            lblProfile.AutoSize = true;
            lblProfile.Location = new Point(3, 0);
            lblProfile.Name = "lblProfile";
            lblProfile.Size = new Size(41, 15);
            lblProfile.TabIndex = 0;
            lblProfile.Text = "Profile";
            // 
            // cboProfile
            // 
            cboProfile.FormattingEnabled = true;
            cboProfile.Location = new Point(115, 2);
            cboProfile.Margin = new Padding(3, 2, 3, 2);
            cboProfile.Name = "cboProfile";
            cboProfile.Size = new Size(275, 23);
            cboProfile.TabIndex = 4;
            cboProfile.SelectedIndexChanged += cboProfile_SelectedIndexChanged;
            // 
            // lblPort
            // 
            lblPort.AutoSize = true;
            lblPort.Location = new Point(3, 108);
            lblPort.Name = "lblPort";
            lblPort.Size = new Size(29, 15);
            lblPort.TabIndex = 0;
            lblPort.Text = "Port";
            // 
            // txtPort
            // 
            txtPort.Location = new Point(115, 110);
            txtPort.Margin = new Padding(3, 2, 3, 2);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(275, 23);
            txtPort.TabIndex = 3;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(351, 193);
            btnSave.Margin = new Padding(3, 2, 3, 2);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(102, 27);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save Profile";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // statusStrip
            // 
            statusStrip.ImageScalingSize = new Size(20, 20);
            statusStrip.Items.AddRange(new ToolStripItem[] { lblVersion, lblStatus });
            statusStrip.Location = new Point(0, 238);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 12, 0);
            statusStrip.Size = new Size(650, 22);
            statusStrip.TabIndex = 8;
            statusStrip.Text = "statusStrip1";
            // 
            // lblVersion
            // 
            lblVersion.Name = "lblVersion";
            lblVersion.Size = new Size(40, 17);
            lblVersion.Text = "0.0.0.1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(19, 17);
            lblStatus.Text = "....";
            // 
            // picLogo
            // 
            picLogo.Image = Properties.Resources.small_logo;
            picLogo.Location = new Point(2, 183);
            picLogo.Margin = new Padding(3, 2, 3, 2);
            picLogo.Name = "picLogo";
            picLogo.Size = new Size(59, 56);
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;
            picLogo.TabIndex = 9;
            picLogo.TabStop = false;
            picLogo.Click += picLogo_Click;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(650, 260);
            Controls.Add(picLogo);
            Controls.Add(statusStrip);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnSave);
            Controls.Add(btnGo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
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
        private Label lblServer;
        private Label lblPassword;
        private TextBox txtPassword;
        private TextBox txtServer;
        private ErrorProvider errorProvider1;
        private Label lblProfile;
        private ComboBox cboProfile;
        private Label lblPort;
        private TextBox txtPort;
        private Button btnSave;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblVersion;
        private ToolStripStatusLabel lblStatus;
        private PictureBox picLogo;
    }
}
