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
            this.dataGridViewAvailableTags = new System.Windows.Forms.DataGridView();
            this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.labelFilter = new System.Windows.Forms.Label();
            this.dataGridViewSelectedTags = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.labelSelectedTags = new System.Windows.Forms.Label();
            this.labelAvailableTags = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAvailableTags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedTags)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewAvailableTags
            // 
            this.dataGridViewAvailableTags.AllowUserToAddRows = false;
            this.dataGridViewAvailableTags.AllowUserToDeleteRows = false;
            this.dataGridViewAvailableTags.AllowUserToResizeRows = false;
            this.dataGridViewAvailableTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAvailableTags.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewAvailableTags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAvailableTags.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnValue});
            this.dataGridViewAvailableTags.Location = new System.Drawing.Point(326, 55);
            this.dataGridViewAvailableTags.Name = "dataGridViewAvailableTags";
            this.dataGridViewAvailableTags.RowHeadersVisible = false;
            this.dataGridViewAvailableTags.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewAvailableTags.Size = new System.Drawing.Size(279, 196);
            this.dataGridViewAvailableTags.TabIndex = 6;
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
            this.textBoxFilter.Location = new System.Drawing.Point(361, 29);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(244, 20);
            this.textBoxFilter.TabIndex = 5;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.TextBoxFilter_TextChanged);
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(323, 32);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(32, 13);
            this.labelFilter.TabIndex = 4;
            this.labelFilter.Text = "Filter:";
            // 
            // dataGridViewSelectedTags
            // 
            this.dataGridViewSelectedTags.AllowUserToAddRows = false;
            this.dataGridViewSelectedTags.AllowUserToDeleteRows = false;
            this.dataGridViewSelectedTags.AllowUserToResizeRows = false;
            this.dataGridViewSelectedTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewSelectedTags.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSelectedTags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSelectedTags.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1});
            this.dataGridViewSelectedTags.Location = new System.Drawing.Point(3, 29);
            this.dataGridViewSelectedTags.Name = "dataGridViewSelectedTags";
            this.dataGridViewSelectedTags.RowHeadersVisible = false;
            this.dataGridViewSelectedTags.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSelectedTags.Size = new System.Drawing.Size(268, 222);
            this.dataGridViewSelectedTags.TabIndex = 7;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Value";
            this.dataGridViewTextBoxColumn1.HeaderText = "Value";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(277, 100);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(43, 23);
            this.buttonAdd.TabIndex = 8;
            this.buttonAdd.Text = "⯇";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.ButtonAdd_Click);
            // 
            // buttonRemove
            // 
            this.buttonRemove.Location = new System.Drawing.Point(277, 129);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(43, 23);
            this.buttonRemove.TabIndex = 9;
            this.buttonRemove.Text = "⯈\t";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.ButtonRemove_Click);
            // 
            // labelSelectedTags
            // 
            this.labelSelectedTags.AutoSize = true;
            this.labelSelectedTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectedTags.Location = new System.Drawing.Point(4, 10);
            this.labelSelectedTags.Name = "labelSelectedTags";
            this.labelSelectedTags.Size = new System.Drawing.Size(89, 13);
            this.labelSelectedTags.TabIndex = 10;
            this.labelSelectedTags.Text = "Selected Tags";
            // 
            // labelAvailableTags
            // 
            this.labelAvailableTags.AutoSize = true;
            this.labelAvailableTags.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAvailableTags.Location = new System.Drawing.Point(323, 10);
            this.labelAvailableTags.Name = "labelAvailableTags";
            this.labelAvailableTags.Size = new System.Drawing.Size(91, 13);
            this.labelAvailableTags.TabIndex = 11;
            this.labelAvailableTags.Text = "Available Tags";
            // 
            // TagSelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelAvailableTags);
            this.Controls.Add(this.labelSelectedTags);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.dataGridViewSelectedTags);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.dataGridViewAvailableTags);
            this.Controls.Add(this.textBoxFilter);
            this.Name = "TagSelectionControl";
            this.Size = new System.Drawing.Size(608, 254);
            this.Load += new System.EventHandler(this.TagSelectionControl_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAvailableTags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSelectedTags)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridViewAvailableTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.DataGridView dataGridViewSelectedTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Label labelSelectedTags;
        private System.Windows.Forms.Label labelAvailableTags;
    }
}
