using Alturos.ImageAnnotation.Contract.Amazon;
using System.Configuration;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class AmazonConfigurationDialog : Form
    {
        public AmazonConfigurationDialog()
        {
            this.InitializeComponent();
        }

        public void SetConfig(AmazonAnnotationPackageProviderConfig config)
        {
            this.textBoxAccessKeyId.Text = config.AccessKeyId;
            this.textBoxSecretAccessKey.Text = config.SecretAccessKey;
            this.textBoxBucketName.Text = config.BucketName;
            this.textBoxDbTableName.Text = config.DbTableName;

            this.textBoxS3ServiceUrl.Text = config.S3ServiceUrl;
            this.textBoxDynamoDbServiceUrl.Text = config.DynamoDbServiceUrl;
        }

        private void SaveConfig()
        {
            var config = this.GetConfig();

            var configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings["accessKeyId"].Value = config.AccessKeyId;
            configuration.AppSettings.Settings["secretAccessKey"].Value = config.SecretAccessKey;
            configuration.AppSettings.Settings["bucketName"].Value = config.BucketName;
            configuration.AppSettings.Settings["dbTableName"].Value = config.DbTableName;

            configuration.AppSettings.Settings["s3ServiceUrl"].Value = config.S3ServiceUrl;
            configuration.AppSettings.Settings["dynamoDbServiceUrl"].Value = config.DynamoDbServiceUrl;

            configuration.Save();

            ConfigurationManager.RefreshSection("appSettings");
        }

        private AmazonAnnotationPackageProviderConfig GetConfig()
        {
            var config = new AmazonAnnotationPackageProviderConfig();
            config.AccessKeyId = this.textBoxAccessKeyId.Text;
            config.SecretAccessKey = this.textBoxSecretAccessKey.Text;
            config.BucketName = this.textBoxBucketName.Text;
            config.DbTableName = this.textBoxDbTableName.Text;

            config.S3ServiceUrl = this.textBoxS3ServiceUrl.Text;
            config.DynamoDbServiceUrl = this.textBoxDynamoDbServiceUrl.Text;

            return config;
        }

        private void buttonSave_Click(object sender, System.EventArgs e)
        {
            this.SaveConfig();
            this.Close();
        }
    }
}
