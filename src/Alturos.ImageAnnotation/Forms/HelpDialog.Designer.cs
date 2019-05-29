namespace Alturos.ImageAnnotation.Forms
{
    partial class HelpDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxAnnotation = new System.Windows.Forms.GroupBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.groupBoxAnnotation.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(389, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Use the keys 0-9 to switch the object class.\r\nAlternatively, use the left and rig" +
    "ht arrow keys to scroll through the object classes.";
            // 
            // groupBoxAnnotation
            // 
            this.groupBoxAnnotation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAnnotation.Controls.Add(this.label1);
            this.groupBoxAnnotation.Location = new System.Drawing.Point(12, 12);
            this.groupBoxAnnotation.Name = "groupBoxAnnotation";
            this.groupBoxAnnotation.Size = new System.Drawing.Size(400, 80);
            this.groupBoxAnnotation.TabIndex = 1;
            this.groupBoxAnnotation.TabStop = false;
            this.groupBoxAnnotation.Text = "Annotation Info";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(337, 98);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // HelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 133);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxAnnotation);
            this.Name = "HelpDialog";
            this.Text = "Help";
            this.groupBoxAnnotation.ResumeLayout(false);
            this.groupBoxAnnotation.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxAnnotation;
        private System.Windows.Forms.Button buttonOk;
    }
}