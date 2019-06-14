namespace Alturos.ImageAnnotation
{
    partial class Main
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
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoplaceAnnotationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPackageStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelImageList = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageImages = new System.Windows.Forms.TabPage();
            this.downloadControl = new Alturos.ImageAnnotation.CustomControls.DownloadControl();
            this.annotationImageListControl = new Alturos.ImageAnnotation.CustomControls.AnnotationImageListControl();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.groupBoxInfo = new System.Windows.Forms.GroupBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelUser = new System.Windows.Forms.Label();
            this.tagEditControl = new Alturos.ImageAnnotation.CustomControls.TagEditControl();
            this.annotationDrawControl = new Alturos.ImageAnnotation.CustomControls.AnnotationDrawControl();
            this.annotationPackageListControl = new Alturos.ImageAnnotation.CustomControls.AnnotationPackageListControl();
            this.menuStripMain.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelImageList.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageImages.SuspendLayout();
            this.tabPageData.SuspendLayout();
            this.groupBoxInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStripMain
            // 
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem,
            this.syncToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.addPackageStripMenuItem,
            this.exportToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStripMain.Location = new System.Drawing.Point(0, 0);
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.Size = new System.Drawing.Size(1334, 24);
            this.menuStripMain.TabIndex = 2;
            this.menuStripMain.Text = "menuStrip1";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.arrow_refresh_small;
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.loadToolStripMenuItem.Text = "&Refresh";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // syncToolStripMenuItem
            // 
            this.syncToolStripMenuItem.Enabled = false;
            this.syncToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.disk;
            this.syncToolStripMenuItem.Name = "syncToolStripMenuItem";
            this.syncToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.syncToolStripMenuItem.Text = "&Save";
            this.syncToolStripMenuItem.Click += new System.EventHandler(this.SyncToolStripMenuItem_Click);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoplaceAnnotationsToolStripMenuItem,
            this.showLabelsToolStripMenuItem,
            this.toolStripSeparator1,
            this.settingsToolStripMenuItem});
            this.configurationToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.wrench;
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(109, 20);
            this.configurationToolStripMenuItem.Text = "&Configuration";
            // 
            // autoplaceAnnotationsToolStripMenuItem
            // 
            this.autoplaceAnnotationsToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.shape_handles;
            this.autoplaceAnnotationsToolStripMenuItem.Name = "autoplaceAnnotationsToolStripMenuItem";
            this.autoplaceAnnotationsToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.autoplaceAnnotationsToolStripMenuItem.Text = "Autoplace Annotations";
            this.autoplaceAnnotationsToolStripMenuItem.Click += new System.EventHandler(this.AutoplaceAnnotationsToolStripMenuItem_Click);
            // 
            // showLabelsToolStripMenuItem
            // 
            this.showLabelsToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.font;
            this.showLabelsToolStripMenuItem.Name = "showLabelsToolStripMenuItem";
            this.showLabelsToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.showLabelsToolStripMenuItem.Text = "Show Object Labels in Image";
            this.showLabelsToolStripMenuItem.Click += new System.EventHandler(this.ShowLabelsToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.cog;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.SettingsToolStripMenuItem_Click);
            // 
            // addPackageStripMenuItem
            // 
            this.addPackageStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.add;
            this.addPackageStripMenuItem.Name = "addPackageStripMenuItem";
            this.addPackageStripMenuItem.Size = new System.Drawing.Size(104, 20);
            this.addPackageStripMenuItem.Text = "&Add Package";
            this.addPackageStripMenuItem.Click += new System.EventHandler(this.AddPackageStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.folder_go;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(141, 20);
            this.exportToolStripMenuItem.Text = "&Export Training Data";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.ExportToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.helpToolStripMenuItem.Image = global::Alturos.ImageAnnotation.Properties.Resources.help;
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.HelpToolStripMenuItem_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 307F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 191F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panelImageList, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.annotationDrawControl, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.annotationPackageListControl, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1334, 530);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // panelImageList
            // 
            this.panelImageList.Controls.Add(this.tabControl);
            this.panelImageList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelImageList.Location = new System.Drawing.Point(310, 3);
            this.panelImageList.Name = "panelImageList";
            this.panelImageList.Size = new System.Drawing.Size(185, 524);
            this.panelImageList.TabIndex = 4;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageImages);
            this.tabControl.Controls.Add(this.tabPageData);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(185, 524);
            this.tabControl.TabIndex = 2;
            // 
            // tabPageImages
            // 
            this.tabPageImages.Controls.Add(this.downloadControl);
            this.tabPageImages.Controls.Add(this.annotationImageListControl);
            this.tabPageImages.Location = new System.Drawing.Point(4, 22);
            this.tabPageImages.Name = "tabPageImages";
            this.tabPageImages.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageImages.Size = new System.Drawing.Size(177, 498);
            this.tabPageImages.TabIndex = 0;
            this.tabPageImages.Text = "Images";
            this.tabPageImages.UseVisualStyleBackColor = true;
            // 
            // downloadControl
            // 
            this.downloadControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downloadControl.BackColor = System.Drawing.SystemColors.Control;
            this.downloadControl.Location = new System.Drawing.Point(-4, 0);
            this.downloadControl.Name = "downloadControl";
            this.downloadControl.Size = new System.Drawing.Size(181, 69);
            this.downloadControl.TabIndex = 1;
            // 
            // annotationImageListControl
            // 
            this.annotationImageListControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationImageListControl.Location = new System.Drawing.Point(3, 3);
            this.annotationImageListControl.Name = "annotationImageListControl";
            this.annotationImageListControl.Size = new System.Drawing.Size(171, 492);
            this.annotationImageListControl.TabIndex = 0;
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.groupBoxInfo);
            this.tabPageData.Controls.Add(this.tagEditControl);
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageData.Size = new System.Drawing.Size(177, 498);
            this.tabPageData.TabIndex = 1;
            this.tabPageData.Text = "Data";
            this.tabPageData.UseVisualStyleBackColor = true;
            // 
            // groupBoxInfo
            // 
            this.groupBoxInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxInfo.Controls.Add(this.labelUserName);
            this.groupBoxInfo.Controls.Add(this.labelUser);
            this.groupBoxInfo.Location = new System.Drawing.Point(6, 114);
            this.groupBoxInfo.Name = "groupBoxInfo";
            this.groupBoxInfo.Size = new System.Drawing.Size(165, 46);
            this.groupBoxInfo.TabIndex = 1;
            this.groupBoxInfo.TabStop = false;
            this.groupBoxInfo.Text = "Info";
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(46, 20);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(0, 13);
            this.labelUserName.TabIndex = 1;
            // 
            // labelUser
            // 
            this.labelUser.AutoSize = true;
            this.labelUser.Location = new System.Drawing.Point(7, 20);
            this.labelUser.Name = "labelUser";
            this.labelUser.Size = new System.Drawing.Size(32, 13);
            this.labelUser.TabIndex = 0;
            this.labelUser.Text = "User:";
            // 
            // tagEditControl
            // 
            this.tagEditControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagEditControl.Location = new System.Drawing.Point(6, 6);
            this.tagEditControl.Name = "tagEditControl";
            this.tagEditControl.Size = new System.Drawing.Size(165, 102);
            this.tagEditControl.TabIndex = 0;
            // 
            // annotationDrawControl
            // 
            this.annotationDrawControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationDrawControl.AutoplaceAnnotations = false;
            this.annotationDrawControl.Location = new System.Drawing.Point(501, 3);
            this.annotationDrawControl.Name = "annotationDrawControl";
            this.annotationDrawControl.Size = new System.Drawing.Size(830, 524);
            this.annotationDrawControl.TabIndex = 2;
            // 
            // annotationPackageListControl
            // 
            this.annotationPackageListControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.annotationPackageListControl.Location = new System.Drawing.Point(3, 3);
            this.annotationPackageListControl.Name = "annotationPackageListControl";
            this.annotationPackageListControl.Size = new System.Drawing.Size(301, 524);
            this.annotationPackageListControl.TabIndex = 1;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1334, 554);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStripMain);
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "Main";
            this.Text = "change by code";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Main_FormClosed);
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelImageList.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabPageImages.ResumeLayout(false);
            this.tabPageData.ResumeLayout(false);
            this.groupBoxInfo.ResumeLayout(false);
            this.groupBoxInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStripMain;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private CustomControls.AnnotationPackageListControl annotationPackageListControl;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private CustomControls.AnnotationDrawControl annotationDrawControl;
        private System.Windows.Forms.Panel panelImageList;
        private System.Windows.Forms.ToolStripMenuItem syncToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPackageStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoplaceAnnotationsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLabelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageImages;
        private CustomControls.DownloadControl downloadControl;
        private CustomControls.AnnotationImageListControl annotationImageListControl;
        private System.Windows.Forms.TabPage tabPageData;
        private CustomControls.TagEditControl tagEditControl;
        private System.Windows.Forms.GroupBox groupBoxInfo;
        private System.Windows.Forms.Label labelUser;
        private System.Windows.Forms.Label labelUserName;
    }
}

