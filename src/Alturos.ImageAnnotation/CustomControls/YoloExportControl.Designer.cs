namespace Alturos.ImageAnnotation.CustomControls
{
    partial class YoloExportControl
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
            this.components = new System.ComponentModel.Container();
            this.groupBoxImageSize = new System.Windows.Forms.GroupBox();
            this.trackBarImageSize = new System.Windows.Forms.TrackBar();
            this.labelImageSize = new System.Windows.Forms.Label();
            this.groupBoxTrainingPercentage = new System.Windows.Forms.GroupBox();
            this.trackBarTrainingPercentage = new System.Windows.Forms.TrackBar();
            this.labelTrainingPercentage = new System.Windows.Forms.Label();
            this.toolTipTrainingPercentage = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipImageSize = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxUseTinyConfig = new System.Windows.Forms.CheckBox();
            this.groupBoxImageSize.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarImageSize)).BeginInit();
            this.groupBoxTrainingPercentage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTrainingPercentage)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxImageSize
            // 
            this.groupBoxImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxImageSize.Controls.Add(this.trackBarImageSize);
            this.groupBoxImageSize.Controls.Add(this.labelImageSize);
            this.groupBoxImageSize.Location = new System.Drawing.Point(292, 3);
            this.groupBoxImageSize.Name = "groupBoxImageSize";
            this.groupBoxImageSize.Size = new System.Drawing.Size(231, 69);
            this.groupBoxImageSize.TabIndex = 17;
            this.groupBoxImageSize.TabStop = false;
            this.groupBoxImageSize.Text = "Image Size";
            // 
            // trackBarImageSize
            // 
            this.trackBarImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarImageSize.LargeChange = 32;
            this.trackBarImageSize.Location = new System.Drawing.Point(6, 19);
            this.trackBarImageSize.Maximum = 608;
            this.trackBarImageSize.Minimum = 416;
            this.trackBarImageSize.Name = "trackBarImageSize";
            this.trackBarImageSize.Size = new System.Drawing.Size(186, 45);
            this.trackBarImageSize.SmallChange = 32;
            this.trackBarImageSize.TabIndex = 12;
            this.trackBarImageSize.TickFrequency = 32;
            this.toolTipImageSize.SetToolTip(this.trackBarImageSize, "The size of the image used for training and testing purposes.\r\nA smaller value wi" +
        "ll require less space, whereas a higher value has the benefit of a more accurate" +
        " learning process.\r\n");
            this.trackBarImageSize.Value = 416;
            this.trackBarImageSize.Scroll += new System.EventHandler(this.TrackBarImageSize_Scroll);
            // 
            // labelImageSize
            // 
            this.labelImageSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelImageSize.AutoSize = true;
            this.labelImageSize.Location = new System.Drawing.Point(198, 35);
            this.labelImageSize.Name = "labelImageSize";
            this.labelImageSize.Size = new System.Drawing.Size(25, 13);
            this.labelImageSize.TabIndex = 13;
            this.labelImageSize.Text = "416";
            // 
            // groupBoxTrainingPercentage
            // 
            this.groupBoxTrainingPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTrainingPercentage.Controls.Add(this.trackBarTrainingPercentage);
            this.groupBoxTrainingPercentage.Controls.Add(this.labelTrainingPercentage);
            this.groupBoxTrainingPercentage.Location = new System.Drawing.Point(3, 3);
            this.groupBoxTrainingPercentage.Name = "groupBoxTrainingPercentage";
            this.groupBoxTrainingPercentage.Size = new System.Drawing.Size(282, 69);
            this.groupBoxTrainingPercentage.TabIndex = 16;
            this.groupBoxTrainingPercentage.TabStop = false;
            this.groupBoxTrainingPercentage.Text = "Training Percentage";
            // 
            // trackBarTrainingPercentage
            // 
            this.trackBarTrainingPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarTrainingPercentage.Location = new System.Drawing.Point(6, 19);
            this.trackBarTrainingPercentage.Maximum = 100;
            this.trackBarTrainingPercentage.Name = "trackBarTrainingPercentage";
            this.trackBarTrainingPercentage.Size = new System.Drawing.Size(237, 45);
            this.trackBarTrainingPercentage.TabIndex = 12;
            this.trackBarTrainingPercentage.TickFrequency = 5;
            this.toolTipTrainingPercentage.SetToolTip(this.trackBarTrainingPercentage, "The percentage stands for how many images each package will randomly select to tr" +
        "ain annotating images.\r\nThe remaining images are used for testing.\r\n");
            this.trackBarTrainingPercentage.Value = 70;
            this.trackBarTrainingPercentage.Scroll += new System.EventHandler(this.TrackBarTrainingPercentage_Scroll);
            // 
            // labelTrainingPercentage
            // 
            this.labelTrainingPercentage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTrainingPercentage.AutoSize = true;
            this.labelTrainingPercentage.Location = new System.Drawing.Point(249, 35);
            this.labelTrainingPercentage.Name = "labelTrainingPercentage";
            this.labelTrainingPercentage.Size = new System.Drawing.Size(27, 13);
            this.labelTrainingPercentage.TabIndex = 13;
            this.labelTrainingPercentage.Text = "70%";
            // 
            // toolTipTrainingPercentage
            // 
            this.toolTipTrainingPercentage.IsBalloon = true;
            this.toolTipTrainingPercentage.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipTrainingPercentage.ToolTipTitle = "Training Percentage";
            // 
            // toolTipImageSize
            // 
            this.toolTipImageSize.IsBalloon = true;
            this.toolTipImageSize.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipImageSize.ToolTipTitle = "Image Size";
            // 
            // checkBoxUseTinyConfig
            // 
            this.checkBoxUseTinyConfig.AutoSize = true;
            this.checkBoxUseTinyConfig.Location = new System.Drawing.Point(3, 78);
            this.checkBoxUseTinyConfig.Name = "checkBoxUseTinyConfig";
            this.checkBoxUseTinyConfig.Size = new System.Drawing.Size(140, 17);
            this.checkBoxUseTinyConfig.TabIndex = 18;
            this.checkBoxUseTinyConfig.Text = "Use Yolo v3 Tiny Config";
            this.checkBoxUseTinyConfig.UseVisualStyleBackColor = true;
            // 
            // YoloExportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxUseTinyConfig);
            this.Controls.Add(this.groupBoxImageSize);
            this.Controls.Add(this.groupBoxTrainingPercentage);
            this.Name = "YoloExportControl";
            this.Size = new System.Drawing.Size(526, 103);
            this.groupBoxImageSize.ResumeLayout(false);
            this.groupBoxImageSize.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarImageSize)).EndInit();
            this.groupBoxTrainingPercentage.ResumeLayout(false);
            this.groupBoxTrainingPercentage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarTrainingPercentage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxImageSize;
        private System.Windows.Forms.TrackBar trackBarImageSize;
        private System.Windows.Forms.Label labelImageSize;
        private System.Windows.Forms.GroupBox groupBoxTrainingPercentage;
        private System.Windows.Forms.TrackBar trackBarTrainingPercentage;
        private System.Windows.Forms.Label labelTrainingPercentage;
        private System.Windows.Forms.ToolTip toolTipTrainingPercentage;
        private System.Windows.Forms.ToolTip toolTipImageSize;
        private System.Windows.Forms.CheckBox checkBoxUseTinyConfig;
    }
}
