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
            this.groupBoxDownload = new System.Windows.Forms.GroupBox();
            this.groupBoxPackages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackages)).BeginInit();
            this.groupBoxDownload.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(6, 32);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(566, 23);
            this.progressBar.TabIndex = 0;
            // 
            // labelSyncing
            // 
            this.labelSyncing.AutoSize = true;
            this.labelSyncing.Location = new System.Drawing.Point(6, 16);
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
            this.buttonSelectFolders.Size = new System.Drawing.Size(563, 23);
            this.buttonSelectFolders.TabIndex = 2;
            this.buttonSelectFolders.Text = "Select Folders";
            this.buttonSelectFolders.UseVisualStyleBackColor = true;
            // 
            // groupBoxPackages
            // 
            this.groupBoxPackages.Controls.Add(this.dataGridViewPackages);
            this.groupBoxPackages.Controls.Add(this.buttonSelectFolders);
            this.groupBoxPackages.Location = new System.Drawing.Point(15, 12);
            this.groupBoxPackages.Name = "groupBoxPackages";
            this.groupBoxPackages.Size = new System.Drawing.Size(575, 179);
            this.groupBoxPackages.TabIndex = 3;
            this.groupBoxPackages.TabStop = false;
            this.groupBoxPackages.Text = "Packages";
            // 
            // dataGridViewPackages
            // 
            this.dataGridViewPackages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPackages.Location = new System.Drawing.Point(6, 49);
            this.dataGridViewPackages.Name = "dataGridViewPackages";
            this.dataGridViewPackages.Size = new System.Drawing.Size(563, 124);
            this.dataGridViewPackages.TabIndex = 3;
            // 
            // groupBoxDownload
            // 
            this.groupBoxDownload.Controls.Add(this.labelSyncing);
            this.groupBoxDownload.Controls.Add(this.progressBar);
            this.groupBoxDownload.Location = new System.Drawing.Point(12, 197);
            this.groupBoxDownload.Name = "groupBoxDownload";
            this.groupBoxDownload.Size = new System.Drawing.Size(578, 61);
            this.groupBoxDownload.TabIndex = 4;
            this.groupBoxDownload.TabStop = false;
            this.groupBoxDownload.Text = "Download";
            // 
            // UploadDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 491);
            this.Controls.Add(this.groupBoxDownload);
            this.Controls.Add(this.groupBoxPackages);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploadDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Uploading";
            this.groupBoxPackages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPackages)).EndInit();
            this.groupBoxDownload.ResumeLayout(false);
            this.groupBoxDownload.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelSyncing;
        private System.Windows.Forms.Button buttonSelectFolders;
        private System.Windows.Forms.GroupBox groupBoxPackages;
        private System.Windows.Forms.DataGridView dataGridViewPackages;
        private System.Windows.Forms.GroupBox groupBoxDownload;
    }
}