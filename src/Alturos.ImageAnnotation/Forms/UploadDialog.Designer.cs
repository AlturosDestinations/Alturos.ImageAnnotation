namespace Alturos.ImageAnnotation.Forms
{
    partial class UploadDialog
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
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelSyncing = new System.Windows.Forms.Label();
            this.buttonSelectFolders = new System.Windows.Forms.Button();
            this.groupBoxPackages = new System.Windows.Forms.GroupBox();
            this.dataGridViewPackages = new System.Windows.Forms.DataGridView();
            this.groupBoxUpload = new System.Windows.Forms.GroupBox();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.groupBoxTags = new System.Windows.Forms.GroupBox();
            this.tagSelectionControl = new Alturos.ImageAnnotation.CustomControls.TagSelectionControl();
            this.groupBoxPackages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackages)).BeginInit();
            this.groupBoxUpload.SuspendLayout();
            this.groupBoxTags.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 83);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(607, 31);
            this.progressBar.TabIndex = 0;
            // 
            // labelSyncing
            // 
            this.labelSyncing.AutoSize = true;
            this.labelSyncing.Location = new System.Drawing.Point(6, 67);
            this.labelSyncing.Name = "labelSyncing";
            this.labelSyncing.Size = new System.Drawing.Size(130, 13);
            this.labelSyncing.TabIndex = 1;
            this.labelSyncing.Text = "Uploading... Please wait...";
            // 
            // buttonSelectFolders
            // 
            this.buttonSelectFolders.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectFolders.Location = new System.Drawing.Point(6, 19);
            this.buttonSelectFolders.Name = "buttonSelectFolders";
            this.buttonSelectFolders.Size = new System.Drawing.Size(598, 23);
            this.buttonSelectFolders.TabIndex = 2;
            this.buttonSelectFolders.Text = "Select Folders";
            this.buttonSelectFolders.UseVisualStyleBackColor = true;
            this.buttonSelectFolders.Click += new System.EventHandler(this.ButtonSelectFolders_Click);
            // 
            // groupBoxPackages
            // 
            this.groupBoxPackages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPackages.Controls.Add(this.dataGridViewPackages);
            this.groupBoxPackages.Controls.Add(this.buttonSelectFolders);
            this.groupBoxPackages.Location = new System.Drawing.Point(15, 12);
            this.groupBoxPackages.Name = "groupBoxPackages";
            this.groupBoxPackages.Size = new System.Drawing.Size(610, 179);
            this.groupBoxPackages.TabIndex = 3;
            this.groupBoxPackages.TabStop = false;
            this.groupBoxPackages.Text = "Packages";
            // 
            // dataGridViewPackages
            // 
            this.dataGridViewPackages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPackages.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPackages.Location = new System.Drawing.Point(6, 49);
            this.dataGridViewPackages.Name = "dataGridViewPackages";
            this.dataGridViewPackages.Size = new System.Drawing.Size(598, 124);
            this.dataGridViewPackages.TabIndex = 3;
            // 
            // groupBoxUpload
            // 
            this.groupBoxUpload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUpload.Controls.Add(this.buttonUpload);
            this.groupBoxUpload.Controls.Add(this.labelSyncing);
            this.groupBoxUpload.Controls.Add(this.progressBar);
            this.groupBoxUpload.Location = new System.Drawing.Point(6, 418);
            this.groupBoxUpload.Name = "groupBoxUpload";
            this.groupBoxUpload.Size = new System.Drawing.Size(619, 120);
            this.groupBoxUpload.TabIndex = 4;
            this.groupBoxUpload.TabStop = false;
            this.groupBoxUpload.Text = "Upload";
            // 
            // buttonUpload
            // 
            this.buttonUpload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUpload.Enabled = false;
            this.buttonUpload.Location = new System.Drawing.Point(6, 19);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(607, 45);
            this.buttonUpload.TabIndex = 2;
            this.buttonUpload.Text = "Upload";
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.ButtonUpload_Click);
            // 
            // groupBoxTags
            // 
            this.groupBoxTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTags.Controls.Add(this.tagSelectionControl);
            this.groupBoxTags.Location = new System.Drawing.Point(15, 197);
            this.groupBoxTags.Name = "groupBoxTags";
            this.groupBoxTags.Size = new System.Drawing.Size(610, 215);
            this.groupBoxTags.TabIndex = 6;
            this.groupBoxTags.TabStop = false;
            this.groupBoxTags.Text = "Tags";
            // 
            // tagSelectionControl
            // 
            this.tagSelectionControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagSelectionControl.Location = new System.Drawing.Point(3, 16);
            this.tagSelectionControl.Name = "tagSelectionControl";
            this.tagSelectionControl.Size = new System.Drawing.Size(604, 196);
            this.tagSelectionControl.TabIndex = 5;
            // 
            // UploadDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 550);
            this.Controls.Add(this.groupBoxTags);
            this.Controls.Add(this.groupBoxUpload);
            this.Controls.Add(this.groupBoxPackages);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploadDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Uploading";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UploadDialog_FormClosed);
            this.Load += new System.EventHandler(this.UploadDialog_Load);
            this.groupBoxPackages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackages)).EndInit();
            this.groupBoxUpload.ResumeLayout(false);
            this.groupBoxUpload.PerformLayout();
            this.groupBoxTags.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelSyncing;
        private System.Windows.Forms.Button buttonSelectFolders;
        private System.Windows.Forms.GroupBox groupBoxPackages;
        private System.Windows.Forms.DataGridView dataGridViewPackages;
        private System.Windows.Forms.GroupBox groupBoxUpload;
        private CustomControls.TagSelectionControl tagSelectionControl;
        private System.Windows.Forms.GroupBox groupBoxTags;
        private System.Windows.Forms.Button buttonUpload;
    }
}