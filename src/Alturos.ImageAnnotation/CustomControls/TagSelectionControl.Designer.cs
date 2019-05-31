namespace Alturos.ImageAnnotation.CustomControls
{
    partial class TagSelectionControl
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
            this.buttonSelect = new System.Windows.Forms.Button();
            this.dataGridViewTags = new System.Windows.Forms.DataGridView();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.labelFilter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTags)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSelect
            // 
            this.buttonSelect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelect.Location = new System.Drawing.Point(3, 265);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(224, 40);
            this.buttonSelect.TabIndex = 7;
            this.buttonSelect.Text = "Add Selected";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
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
            this.dataGridViewTags.Location = new System.Drawing.Point(3, 26);
            this.dataGridViewTags.Name = "dataGridViewTags";
            this.dataGridViewTags.RowHeadersVisible = false;
            this.dataGridViewTags.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTags.Size = new System.Drawing.Size(224, 233);
            this.dataGridViewTags.TabIndex = 6;
            // 
            // ColumnValue
            // 
            this.ColumnValue.DataPropertyName = "Value";
            this.ColumnValue.HeaderText = "Value";
            this.ColumnValue.Name = "ColumnValue";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilter.Location = new System.Drawing.Point(41, 0);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(186, 20);
            this.textBoxFilter.TabIndex = 5;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.TextBoxFilter_TextChanged);
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(3, 3);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(32, 13);
            this.labelFilter.TabIndex = 4;
            this.labelFilter.Text = "Filter:";
            // 
            // TagSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonSelect);
            this.Controls.Add(this.dataGridViewTags);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.labelFilter);
            this.Name = "TagSelectionControl";
            this.Size = new System.Drawing.Size(230, 308);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTags)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.DataGridView dataGridViewTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label labelFilter;
    }
}
