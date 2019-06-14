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
            this.groupBoxBboxEditing = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelKeyboardShortcuts = new System.Windows.Forms.Label();
            this.groupBoxAnnotation.SuspendLayout();
            this.groupBoxBboxEditing.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "Arrow Key Up / Down: Select image\r\nArrow Key Left / Right or 0 - 9: Select object" +
    " class";
            // 
            // groupBoxAnnotation
            // 
            this.groupBoxAnnotation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAnnotation.Controls.Add(this.label1);
            this.groupBoxAnnotation.Location = new System.Drawing.Point(12, 36);
            this.groupBoxAnnotation.Name = "groupBoxAnnotation";
            this.groupBoxAnnotation.Size = new System.Drawing.Size(490, 54);
            this.groupBoxAnnotation.TabIndex = 1;
            this.groupBoxAnnotation.TabStop = false;
            this.groupBoxAnnotation.Text = "Annotation Info";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(427, 208);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "OK";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // groupBoxBboxEditing
            // 
            this.groupBoxBboxEditing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxBboxEditing.Controls.Add(this.label2);
            this.groupBoxBboxEditing.Location = new System.Drawing.Point(12, 102);
            this.groupBoxBboxEditing.Name = "groupBoxBboxEditing";
            this.groupBoxBboxEditing.Size = new System.Drawing.Size(490, 95);
            this.groupBoxBboxEditing.TabIndex = 2;
            this.groupBoxBboxEditing.TabStop = false;
            this.groupBoxBboxEditing.Text = "Bounding Box Editing";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(291, 65);
            this.label2.TabIndex = 0;
            this.label2.Text = "WASD: Move image\r\nShift + WASD: Resize image (moving bottom-right boundary)\r\nShif" +
    "t + Alt + WASD: Resize image (moving top-left boundary)\r\n\r\nHold Ctrl to move or " +
    "resize quickly.";
            // 
            // labelKeyboardShortcuts
            // 
            this.labelKeyboardShortcuts.AutoSize = true;
            this.labelKeyboardShortcuts.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelKeyboardShortcuts.Location = new System.Drawing.Point(12, 9);
            this.labelKeyboardShortcuts.Name = "labelKeyboardShortcuts";
            this.labelKeyboardShortcuts.Size = new System.Drawing.Size(118, 13);
            this.labelKeyboardShortcuts.TabIndex = 3;
            this.labelKeyboardShortcuts.Text = "Keyboard Shortcuts";
            // 
            // HelpDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 243);
            this.Controls.Add(this.labelKeyboardShortcuts);
            this.Controls.Add(this.groupBoxBboxEditing);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxAnnotation);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HelpDialog";
            this.Text = "Help";
            this.groupBoxAnnotation.ResumeLayout(false);
            this.groupBoxAnnotation.PerformLayout();
            this.groupBoxBboxEditing.ResumeLayout(false);
            this.groupBoxBboxEditing.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxAnnotation;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.GroupBox groupBoxBboxEditing;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelKeyboardShortcuts;
    }
}