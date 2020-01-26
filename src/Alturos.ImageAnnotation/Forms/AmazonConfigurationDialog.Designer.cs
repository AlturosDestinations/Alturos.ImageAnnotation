namespace Alturos.ImageAnnotation.Forms
{
    partial class AmazonConfigurationDialog
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
            this.textBoxAccessKeyId = new System.Windows.Forms.TextBox();
            this.labelAccessKey = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSecretAccessKey = new System.Windows.Forms.TextBox();
            this.textBoxBucketName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDbTableName = new System.Windows.Forms.TextBox();
            this.groupBoxLocalInstallation = new System.Windows.Forms.GroupBox();
            this.textBoxS3ServiceUrl = new System.Windows.Forms.TextBox();
            this.textBoxDynamoDbServiceUrl = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBoxLocalInstallation.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxAccessKeyId
            // 
            this.textBoxAccessKeyId.Location = new System.Drawing.Point(127, 26);
            this.textBoxAccessKeyId.Name = "textBoxAccessKeyId";
            this.textBoxAccessKeyId.Size = new System.Drawing.Size(277, 20);
            this.textBoxAccessKeyId.TabIndex = 0;
            // 
            // labelAccessKey
            // 
            this.labelAccessKey.AutoSize = true;
            this.labelAccessKey.Location = new System.Drawing.Point(49, 29);
            this.labelAccessKey.Name = "labelAccessKey";
            this.labelAccessKey.Size = new System.Drawing.Size(72, 13);
            this.labelAccessKey.TabIndex = 1;
            this.labelAccessKey.Text = "AccessKeyId:";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(357, 269);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "SecretAccessKey:";
            // 
            // textBoxSecretAccessKey
            // 
            this.textBoxSecretAccessKey.Location = new System.Drawing.Point(127, 52);
            this.textBoxSecretAccessKey.Name = "textBoxSecretAccessKey";
            this.textBoxSecretAccessKey.Size = new System.Drawing.Size(277, 20);
            this.textBoxSecretAccessKey.TabIndex = 4;
            // 
            // textBoxBucketName
            // 
            this.textBoxBucketName.Location = new System.Drawing.Point(127, 78);
            this.textBoxBucketName.Name = "textBoxBucketName";
            this.textBoxBucketName.Size = new System.Drawing.Size(277, 20);
            this.textBoxBucketName.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "BucketName:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "DbTableName:";
            // 
            // textBoxDbTableName
            // 
            this.textBoxDbTableName.Location = new System.Drawing.Point(127, 104);
            this.textBoxDbTableName.Name = "textBoxDbTableName";
            this.textBoxDbTableName.Size = new System.Drawing.Size(277, 20);
            this.textBoxDbTableName.TabIndex = 8;
            // 
            // groupBoxLocalInstallation
            // 
            this.groupBoxLocalInstallation.Controls.Add(this.label5);
            this.groupBoxLocalInstallation.Controls.Add(this.label4);
            this.groupBoxLocalInstallation.Controls.Add(this.textBoxDynamoDbServiceUrl);
            this.groupBoxLocalInstallation.Controls.Add(this.textBoxS3ServiceUrl);
            this.groupBoxLocalInstallation.Location = new System.Drawing.Point(12, 154);
            this.groupBoxLocalInstallation.Name = "groupBoxLocalInstallation";
            this.groupBoxLocalInstallation.Size = new System.Drawing.Size(420, 109);
            this.groupBoxLocalInstallation.TabIndex = 9;
            this.groupBoxLocalInstallation.TabStop = false;
            this.groupBoxLocalInstallation.Text = "Local Installation (Optional configuration)";
            // 
            // textBoxS3ServiceUrl
            // 
            this.textBoxS3ServiceUrl.Location = new System.Drawing.Point(127, 33);
            this.textBoxS3ServiceUrl.Name = "textBoxS3ServiceUrl";
            this.textBoxS3ServiceUrl.Size = new System.Drawing.Size(277, 20);
            this.textBoxS3ServiceUrl.TabIndex = 10;
            // 
            // textBoxDynamoDbServiceUrl
            // 
            this.textBoxDynamoDbServiceUrl.Location = new System.Drawing.Point(127, 59);
            this.textBoxDynamoDbServiceUrl.Name = "textBoxDynamoDbServiceUrl";
            this.textBoxDynamoDbServiceUrl.Size = new System.Drawing.Size(277, 20);
            this.textBoxDynamoDbServiceUrl.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "S3ServiceUrl:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "DynamoDbServiceUrl:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelAccessKey);
            this.groupBox1.Controls.Add(this.textBoxAccessKeyId);
            this.groupBox1.Controls.Add(this.textBoxDbTableName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxSecretAccessKey);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxBucketName);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 136);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Basic configuration (required)";
            // 
            // AmazonConfigurationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 298);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBoxLocalInstallation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AmazonConfigurationDialog";
            this.Text = "Configuration";
            this.groupBoxLocalInstallation.ResumeLayout(false);
            this.groupBoxLocalInstallation.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxAccessKeyId;
        private System.Windows.Forms.Label labelAccessKey;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSecretAccessKey;
        private System.Windows.Forms.TextBox textBoxBucketName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDbTableName;
        private System.Windows.Forms.GroupBox groupBoxLocalInstallation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDynamoDbServiceUrl;
        private System.Windows.Forms.TextBox textBoxS3ServiceUrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}