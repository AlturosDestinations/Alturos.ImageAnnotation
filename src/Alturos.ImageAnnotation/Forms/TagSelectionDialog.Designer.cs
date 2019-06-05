namespace Alturos.ImageAnnotation.Forms
{
    partial class TagSelectionDialog
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
            this.tagSelectionControl = new Alturos.ImageAnnotation.CustomControls.TagSelectionControl();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tagSelectionControl
            // 
            this.tagSelectionControl.Location = new System.Drawing.Point(12, 12);
            this.tagSelectionControl.Name = "tagSelectionControl";
            this.tagSelectionControl.Size = new System.Drawing.Size(610, 254);
            this.tagSelectionControl.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(507, 276);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(115, 29);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // TagSelectionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 317);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.tagSelectionControl);
            this.Name = "TagSelectionDialog";
            this.Text = "TagSelectionForm";
            this.Load += new System.EventHandler(this.TagSelectionDialog_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControls.TagSelectionControl tagSelectionControl;
        private System.Windows.Forms.Button buttonClose;
    }
}