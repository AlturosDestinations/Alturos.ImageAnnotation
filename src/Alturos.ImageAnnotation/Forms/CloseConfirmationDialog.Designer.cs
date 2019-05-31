namespace Alturos.ImageAnnotation.Forms
{
    partial class CloseConfirmationDialog
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
            this.labelUpperDescription = new System.Windows.Forms.Label();
            this.buttonDiscard = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelUpperDescription
            // 
            this.labelUpperDescription.AutoSize = true;
            this.labelUpperDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUpperDescription.Location = new System.Drawing.Point(13, 13);
            this.labelUpperDescription.Name = "labelUpperDescription";
            this.labelUpperDescription.Size = new System.Drawing.Size(422, 32);
            this.labelUpperDescription.TabIndex = 0;
            this.labelUpperDescription.Text = "There are unsynced packages!\r\nDo you really want to quit and discard all unsaved " +
    "changes?";
            // 
            // buttonDiscard
            // 
            this.buttonDiscard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonDiscard.BackColor = System.Drawing.Color.Red;
            this.buttonDiscard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDiscard.ForeColor = System.Drawing.Color.White;
            this.buttonDiscard.Location = new System.Drawing.Point(162, 67);
            this.buttonDiscard.Name = "buttonDiscard";
            this.buttonDiscard.Padding = new System.Windows.Forms.Padding(5);
            this.buttonDiscard.Size = new System.Drawing.Size(137, 30);
            this.buttonDiscard.TabIndex = 1;
            this.buttonDiscard.Text = "Discard Changes";
            this.buttonDiscard.UseVisualStyleBackColor = false;
            this.buttonDiscard.Click += new System.EventHandler(this.ButtonDiscard_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.ForeColor = System.Drawing.Color.Black;
            this.buttonCancel.Location = new System.Drawing.Point(305, 67);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Padding = new System.Windows.Forms.Padding(5);
            this.buttonCancel.Size = new System.Drawing.Size(136, 30);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // CloseConfirmationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(453, 109);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonDiscard);
            this.Controls.Add(this.labelUpperDescription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CloseConfirmationDialog";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Confirm Closing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUpperDescription;
        private System.Windows.Forms.Button buttonDiscard;
        private System.Windows.Forms.Button buttonCancel;
    }
}