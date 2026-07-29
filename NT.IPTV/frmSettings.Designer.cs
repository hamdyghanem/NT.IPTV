namespace NT.IPTV
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            groupBoxDownload = new GroupBox();
            lblDownloadFolder = new Label();
            txtDownloadFolder = new TextBox();
            btnBrowse = new Button();
            btnSave = new Button();
            btnCancel = new Button();

            groupBoxDownload.SuspendLayout();
            SuspendLayout();

            // groupBoxDownload
            groupBoxDownload.Controls.Add(lblDownloadFolder);
            groupBoxDownload.Controls.Add(txtDownloadFolder);
            groupBoxDownload.Controls.Add(btnBrowse);
            groupBoxDownload.Location = new Point(12, 12);
            groupBoxDownload.Name = "groupBoxDownload";
            groupBoxDownload.Padding = new Padding(10);
            groupBoxDownload.Size = new Size(460, 120);
            groupBoxDownload.TabIndex = 0;
            groupBoxDownload.TabStop = false;
            groupBoxDownload.Text = "Download Settings";

            // lblDownloadFolder
            lblDownloadFolder.AutoSize = true;
            lblDownloadFolder.Location = new Point(19, 30);
            lblDownloadFolder.Name = "lblDownloadFolder";
            lblDownloadFolder.Size = new Size(104, 20);
            lblDownloadFolder.TabIndex = 0;
            lblDownloadFolder.Text = "Download Folder:";

            // txtDownloadFolder
            txtDownloadFolder.Location = new Point(19, 55);
            txtDownloadFolder.Name = "txtDownloadFolder";
            txtDownloadFolder.ReadOnly = true;
            txtDownloadFolder.Size = new Size(325, 27);
            txtDownloadFolder.TabIndex = 1;

            // btnBrowse
            btnBrowse.Location = new Point(350, 55);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(90, 27);
            btnBrowse.TabIndex = 2;
            btnBrowse.Text = "Browse...";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;

            // btnSave
            btnSave.Location = new Point(316, 150);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 30);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;

            // btnCancel
            btnCancel.Location = new Point(397, 150);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 30);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            // frmSettings
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(484, 192);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(groupBoxDownload);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmSettings";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Application Settings";

            groupBoxDownload.ResumeLayout(false);
            groupBoxDownload.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBoxDownload;
        private Label lblDownloadFolder;
        private TextBox txtDownloadFolder;
        private Button btnBrowse;
        private Button btnSave;
        private Button btnCancel;
    }
}
