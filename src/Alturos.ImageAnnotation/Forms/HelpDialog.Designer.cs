﻿namespace Alturos.ImageAnnotation.Forms
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
            this.label1.Size = new System.Drawing.Size(200, 91);
            this.label1.TabIndex = 0;
            this.label1.Text = "Up / Down: Select Image\r\nLeft / Right or 0 - 9: Switch Object Class\r\n\r\nWASD: Move" +
    " image\r\nShift + WASD: Move image slowly\r\nCtrl + WASD: Resize image\r\nShift + Ctrl" +
    " + WASD: Resize image slowly";
            // 
            // groupBoxAnnotation
            // 
            this.groupBoxAnnotation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAnnotation.Controls.Add(this.label1);
            this.groupBoxAnnotation.Location = new System.Drawing.Point(12, 12);
            this.groupBoxAnnotation.Name = "groupBoxAnnotation";
            this.groupBoxAnnotation.Size = new System.Drawing.Size(337, 127);
            this.groupBoxAnnotation.TabIndex = 1;
            this.groupBoxAnnotation.TabStop = false;
            this.groupBoxAnnotation.Text = "Annotation Info";
            // 
            // buttonOk
            // 
            this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOk.Location = new System.Drawing.Point(274, 145);
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
            this.ClientSize = new System.Drawing.Size(361, 180);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.groupBoxAnnotation);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
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