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
            pictureBox1 = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // btnGo
            // 
            btnGo.Location = new Point(380, 244);
            btnGo.Name = "btnGo";
            btnGo.Size = new Size(117, 36);
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
            tableLayoutPanel1.Location = new Point(113, 23);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 6;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(567, 215);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(132, 37);
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
            // lblServer
            // 
            lblServer.AutoSize = true;
            lblServer.Location = new Point(3, 100);
            lblServer.Name = "lblServer";
            lblServer.Size = new Size(50, 20);
            lblServer.TabIndex = 0;
            lblServer.Text = "Server";
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
            txtPassword.Location = new Point(132, 70);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(314, 27);
            txtPassword.TabIndex = 2;
            // 
            // txtServer
            // 
            txtServer.Location = new Point(132, 103);
            txtServer.Name = "txtServer";
            txtServer.Size = new Size(314, 27);
            txtServer.TabIndex = 3;
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
            cboProfile.Location = new Point(132, 3);
            cboProfile.Name = "cboProfile";
            cboProfile.Size = new Size(314, 28);
            cboProfile.TabIndex = 4;
            cboProfile.SelectedIndexChanged += cboProfile_SelectedIndexChanged;
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
            txtPort.Location = new Point(132, 136);
            txtPort.Name = "txtPort";
            txtPort.Size = new Size(314, 27);
            txtPort.TabIndex = 3;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(257, 244);
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
            statusStrip.Location = new Point(0, 321);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(743, 26);
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
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.small_logo;
            pictureBox1.Location = new Point(2, 244);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(67, 74);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(743, 347);
            Controls.Add(pictureBox1);
            Controls.Add(statusStrip);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(btnSave);
            Controls.Add(btnGo);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
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
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
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
        private PictureBox pictureBox1;
    }
}
