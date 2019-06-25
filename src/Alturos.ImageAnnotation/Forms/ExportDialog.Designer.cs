namespace Alturos.ImageAnnotation.Forms
{
    partial class ExportDialog
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
            this.buttonExport = new System.Windows.Forms.Button();
            this.dataGridViewTags = new System.Windows.Forms.DataGridView();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewResult = new System.Windows.Forms.DataGridView();
            this.ColumnName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnExported = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.groupBoxTags = new System.Windows.Forms.GroupBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.groupBoxResult = new System.Windows.Forms.GroupBox();
            this.labelPackageCount = new System.Windows.Forms.Label();
            this.comboBoxExportProvider = new System.Windows.Forms.ComboBox();
            this.groupBoxExportProvider = new System.Windows.Forms.GroupBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelDownloadProgress = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxObjectClasses = new System.Windows.Forms.GroupBox();
            this.dataGridViewObjectClasses = new System.Windows.Forms.DataGridView();
            this.ColumnChecked = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnObjectClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelProviderSettings = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).BeginInit();
            this.groupBoxTags.SuspendLayout();
            this.groupBoxResult.SuspendLayout();
            this.groupBoxExportProvider.SuspendLayout();
            this.groupBoxObjectClasses.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewObjectClasses)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(12, 563);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(564, 41);
            this.buttonExport.TabIndex = 3;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.ButtonExport_Click);
            // 
            // dataGridViewTags
            // 
            this.dataGridViewTags.AllowUserToAddRows = false;
            this.dataGridViewTags.AllowUserToDeleteRows = false;
            this.dataGridViewTags.AllowUserToResizeRows = false;
            this.dataGridViewTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTags.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTags.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnValue});
            this.dataGridViewTags.Location = new System.Drawing.Point(6, 19);
            this.dataGridViewTags.Name = "dataGridViewTags";
            this.dataGridViewTags.ReadOnly = true;
            this.dataGridViewTags.RowHeadersVisible = false;
            this.dataGridViewTags.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTags.Size = new System.Drawing.Size(289, 112);
            this.dataGridViewTags.TabIndex = 4;
            // 
            // ColumnValue
            // 
            this.ColumnValue.DataPropertyName = "Value";
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            this.ColumnValue.ReadOnly = true;
            // 
            // dataGridViewResult
            // 
            this.dataGridViewResult.AllowUserToAddRows = false;
            this.dataGridViewResult.AllowUserToDeleteRows = false;
            this.dataGridViewResult.AllowUserToResizeRows = false;
            this.dataGridViewResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResult.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnName,
            this.ColumnExported});
            this.dataGridViewResult.Location = new System.Drawing.Point(3, 16);
            this.dataGridViewResult.Name = "dataGridViewResult";
            this.dataGridViewResult.ReadOnly = true;
            this.dataGridViewResult.RowHeadersVisible = false;
            this.dataGridViewResult.Size = new System.Drawing.Size(555, 175);
            this.dataGridViewResult.TabIndex = 5;
            // 
            // ColumnName
            // 
            this.ColumnName.DataPropertyName = "PackageName";
            this.ColumnName.FillWeight = 164.467F;
            this.ColumnName.HeaderText = "Package Name";
            this.ColumnName.Name = "ColumnName";
            this.ColumnName.ReadOnly = true;
            // 
            // ColumnExported
            // 
            this.ColumnExported.DataPropertyName = "AvailableLocally";
            this.ColumnExported.FillWeight = 35.533F;
            this.ColumnExported.HeaderText = "Available Locally";
            this.ColumnExported.Name = "ColumnExported";
            this.ColumnExported.ReadOnly = true;
            // 
            // groupBoxTags
            // 
            this.groupBoxTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTags.Controls.Add(this.buttonSearch);
            this.groupBoxTags.Controls.Add(this.dataGridViewTags);
            this.groupBoxTags.Location = new System.Drawing.Point(12, 9);
            this.groupBoxTags.Name = "groupBoxTags";
            this.groupBoxTags.Size = new System.Drawing.Size(301, 167);
            this.groupBoxTags.TabIndex = 6;
            this.groupBoxTags.TabStop = false;
            this.groupBoxTags.Text = "Tags";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.Location = new System.Drawing.Point(220, 137);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 24);
            this.buttonSearch.TabIndex = 8;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.ButtonSearch_Click);
            // 
            // groupBoxResult
            // 
            this.groupBoxResult.Controls.Add(this.labelPackageCount);
            this.groupBoxResult.Controls.Add(this.dataGridViewResult);
            this.groupBoxResult.Location = new System.Drawing.Point(12, 182);
            this.groupBoxResult.Name = "groupBoxResult";
            this.groupBoxResult.Size = new System.Drawing.Size(564, 210);
            this.groupBoxResult.TabIndex = 7;
            this.groupBoxResult.TabStop = false;
            this.groupBoxResult.Text = "Result";
            // 
            // labelPackageCount
            // 
            this.labelPackageCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPackageCount.AutoSize = true;
            this.labelPackageCount.Location = new System.Drawing.Point(6, 194);
            this.labelPackageCount.Name = "labelPackageCount";
            this.labelPackageCount.Size = new System.Drawing.Size(13, 13);
            this.labelPackageCount.TabIndex = 9;
            this.labelPackageCount.Text = "0";
            // 
            // comboBoxExportProvider
            // 
            this.comboBoxExportProvider.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBoxExportProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxExportProvider.FormattingEnabled = true;
            this.comboBoxExportProvider.Location = new System.Drawing.Point(3, 19);
            this.comboBoxExportProvider.Name = "comboBoxExportProvider";
            this.comboBoxExportProvider.Size = new System.Drawing.Size(558, 21);
            this.comboBoxExportProvider.TabIndex = 11;
            this.comboBoxExportProvider.SelectedIndexChanged += new System.EventHandler(this.ComboBoxExportProvider_SelectedIndexChanged);
            // 
            // groupBoxExportProvider
            // 
            this.groupBoxExportProvider.Controls.Add(this.comboBoxExportProvider);
            this.groupBoxExportProvider.Location = new System.Drawing.Point(12, 398);
            this.groupBoxExportProvider.Name = "groupBoxExportProvider";
            this.groupBoxExportProvider.Padding = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.groupBoxExportProvider.Size = new System.Drawing.Size(564, 50);
            this.groupBoxExportProvider.TabIndex = 5;
            this.groupBoxExportProvider.TabStop = false;
            this.groupBoxExportProvider.Text = "Export Provider";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(9, 623);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(475, 31);
            this.progressBar.TabIndex = 8;
            // 
            // labelDownloadProgress
            // 
            this.labelDownloadProgress.AutoSize = true;
            this.labelDownloadProgress.Location = new System.Drawing.Point(9, 607);
            this.labelDownloadProgress.Name = "labelDownloadProgress";
            this.labelDownloadProgress.Size = new System.Drawing.Size(126, 13);
            this.labelDownloadProgress.TabIndex = 9;
            this.labelDownloadProgress.Text = "Exporting... Please wait...";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Enabled = false;
            this.buttonCancel.Location = new System.Drawing.Point(493, 623);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(83, 31);
            this.buttonCancel.TabIndex = 10;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // groupBoxObjectClasses
            // 
            this.groupBoxObjectClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxObjectClasses.Controls.Add(this.dataGridViewObjectClasses);
            this.groupBoxObjectClasses.Location = new System.Drawing.Point(320, 9);
            this.groupBoxObjectClasses.Name = "groupBoxObjectClasses";
            this.groupBoxObjectClasses.Size = new System.Drawing.Size(256, 167);
            this.groupBoxObjectClasses.TabIndex = 11;
            this.groupBoxObjectClasses.TabStop = false;
            this.groupBoxObjectClasses.Text = "Object Classes";
            // 
            // dataGridViewObjectClasses
            // 
            this.dataGridViewObjectClasses.AllowUserToAddRows = false;
            this.dataGridViewObjectClasses.AllowUserToDeleteRows = false;
            this.dataGridViewObjectClasses.AllowUserToResizeRows = false;
            this.dataGridViewObjectClasses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewObjectClasses.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewObjectClasses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewObjectClasses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnChecked,
            this.ColumnId,
            this.ColumnObjectClass});
            this.dataGridViewObjectClasses.Location = new System.Drawing.Point(6, 15);
            this.dataGridViewObjectClasses.Name = "dataGridViewObjectClasses";
            this.dataGridViewObjectClasses.RowHeadersVisible = false;
            this.dataGridViewObjectClasses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewObjectClasses.Size = new System.Drawing.Size(244, 146);
            this.dataGridViewObjectClasses.TabIndex = 9;
            // 
            // ColumnChecked
            // 
            this.ColumnChecked.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnChecked.DataPropertyName = "Selected";
            this.ColumnChecked.FillWeight = 90.63071F;
            this.ColumnChecked.HeaderText = "";
            this.ColumnChecked.Name = "ColumnChecked";
            this.ColumnChecked.Width = 50;
            // 
            // ColumnId
            // 
            this.ColumnId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnId.DataPropertyName = "Id";
            this.ColumnId.FillWeight = 76.14214F;
            this.ColumnId.HeaderText = "Id";
            this.ColumnId.Name = "ColumnId";
            this.ColumnId.ReadOnly = true;
            this.ColumnId.Width = 50;
            // 
            // ColumnObjectClass
            // 
            this.ColumnObjectClass.DataPropertyName = "Name";
            this.ColumnObjectClass.FillWeight = 133.2271F;
            this.ColumnObjectClass.HeaderText = "Name";
            this.ColumnObjectClass.Name = "ColumnObjectClass";
            this.ColumnObjectClass.ReadOnly = true;
            // 
            // panelProviderSettings
            // 
            this.panelProviderSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelProviderSettings.Location = new System.Drawing.Point(12, 454);
            this.panelProviderSettings.Name = "panelProviderSettings";
            this.panelProviderSettings.Size = new System.Drawing.Size(564, 100);
            this.panelProviderSettings.TabIndex = 12;
            // 
            // ExportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(588, 665);
            this.Controls.Add(this.panelProviderSettings);
            this.Controls.Add(this.groupBoxObjectClasses);
            this.Controls.Add(this.groupBoxExportProvider);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelDownloadProgress);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBoxTags);
            this.Controls.Add(this.groupBoxResult);
            this.Controls.Add(this.buttonExport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Training Data";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExportDialog_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResult)).EndInit();
            this.groupBoxTags.ResumeLayout(false);
            this.groupBoxResult.ResumeLayout(false);
            this.groupBoxResult.PerformLayout();
            this.groupBoxExportProvider.ResumeLayout(false);
            this.groupBoxObjectClasses.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewObjectClasses)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.DataGridView dataGridViewTags;
        private System.Windows.Forms.DataGridView dataGridViewResult;
        private System.Windows.Forms.GroupBox groupBoxTags;
        private System.Windows.Forms.GroupBox groupBoxResult;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Label labelPackageCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.ComboBox comboBoxExportProvider;
        private System.Windows.Forms.GroupBox groupBoxExportProvider;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelDownloadProgress;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxObjectClasses;
        private System.Windows.Forms.DataGridView dataGridViewObjectClasses;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnChecked;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnObjectClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnName;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnExported;
        private System.Windows.Forms.Panel panelProviderSettings;
    }
}