namespace NT.IPTV
{
    partial class RowCatControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RowCatControl));
            lblName = new Label();
            pnlTool = new Panel();
            btnSettings = new PictureBox();
            btnFavorite = new PictureBox();
            pnlTool.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)btnSettings).BeginInit();
            ((System.ComponentModel.ISupportInitialize)btnFavorite).BeginInit();
            SuspendLayout();
            // 
            // lblName
            // 
            lblName.BackColor = Color.Transparent;
            lblName.Dock = DockStyle.Fill;
            lblName.Font = new Font("Segoe UI", 10F);
            lblName.ForeColor = Color.White;
            lblName.Location = new Point(92, 0);
            lblName.Name = "lblName";
            lblName.Padding = new Padding(5);
            lblName.Size = new Size(556, 43);
            lblName.TabIndex = 2;
            lblName.Text = "Series Name";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            lblName.Click += lblName_Click;
            lblName.MouseEnter += lblName_MouseEnter;
            lblName.MouseLeave += lblName_MouseLeave;
            // 
            // pnlTool
            // 
            pnlTool.Controls.Add(btnSettings);
            pnlTool.Controls.Add(btnFavorite);
            pnlTool.Dock = DockStyle.Left;
            pnlTool.Location = new Point(0, 0);
            pnlTool.Name = "pnlTool";
            pnlTool.Size = new Size(92, 43);
            pnlTool.TabIndex = 3;
            // 
            // btnSettings
            // 
            btnSettings.BackColor = Color.Black;
            btnSettings.BackgroundImage = (Image)resources.GetObject("btnSettings.BackgroundImage");
            btnSettings.BackgroundImageLayout = ImageLayout.Stretch;
            btnSettings.Dock = DockStyle.Left;
            btnSettings.Location = new Point(44, 0);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(44, 43);
            btnSettings.TabIndex = 0;
            btnSettings.TabStop = false;
            btnSettings.Click += btnSettings_Click;
            // 
            // btnFavorite
            // 
            btnFavorite.BackgroundImage = Properties.Resources.RatingDown;
            btnFavorite.BackgroundImageLayout = ImageLayout.Center;
            btnFavorite.Dock = DockStyle.Left;
            btnFavorite.Location = new Point(0, 0);
            btnFavorite.Name = "btnFavorite";
            btnFavorite.Size = new Size(44, 43);
            btnFavorite.TabIndex = 0;
            btnFavorite.TabStop = false;
            btnFavorite.Click += btnFavorite_Click;
            // 
            // RowCatControl
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            Controls.Add(lblName);
            Controls.Add(pnlTool);
            Name = "RowCatControl";
            Size = new Size(648, 43);
            ForeColorChanged += RowCatControl_ForeColorChanged;
            pnlTool.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)btnSettings).EndInit();
            ((System.ComponentModel.ISupportInitialize)btnFavorite).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Label lblName;
        private Panel pnlTool;
        private PictureBox btnSettings;
        private PictureBox btnFavorite;
    }
}
