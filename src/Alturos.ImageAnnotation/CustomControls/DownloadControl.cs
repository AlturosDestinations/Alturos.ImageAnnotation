using Alturos.ImageAnnotation.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class DownloadControl : UserControl
    {
        public event Func<AnnotationPackage, Task> DownloadRequested;

        private AnnotationPackage _packageToExtract;

        public DownloadControl()
        {
            this.InitializeComponent();
            this.labelDownload.Text = "";
        }

        public void ShowDownloadDialog(AnnotationPackage package)
        {
            this.buttonDownload.Visible = !package.Downloading;
            this.progressBarDownload.Visible = package.Downloading;
            this.labelPercentage.Visible = package.Downloading;
            this.labelNotification.Text = package.Downloading ?
                $"Downloading {package.PackageName}. Please wait..." :
                "The package is not available locally yet. Please download it first";

            this._packageToExtract = package;

            this.labelPercentage.Text = "";
            this.labelDownload.Text = "";
            this.progressBarDownload.Value = (int)package.DownloadProgress;

            this.Show();

            Task.Run(() => this.ShowDownloadProgress(package));
        }

        private async Task ShowDownloadProgress(AnnotationPackage package)
        {
            while (package.DownloadProgress < 100 && package.Downloading && this._packageToExtract == package)
            {
                this.labelPercentage.Invoke((MethodInvoker)delegate
                {
                    this.labelPercentage.Text = $"{(int)package.DownloadProgress}%";
                });
                this.labelDownload.Invoke((MethodInvoker)delegate
                {
                    this.labelDownload.Text = $"{package.TransferredBytes / 1024.0 / 1024.0:0.00} MB of {package.TotalBytes / 1024.0 / 1024.0:0.00} MB";
                });
                this.progressBarDownload.Invoke((MethodInvoker)delegate { this.progressBarDownload.Value = (int)package.DownloadProgress; });
                await Task.Delay(200);
            }
        }

        private async void buttonDownload_Click(object sender, EventArgs e)
        {
            this._packageToExtract.Downloading = true;

            this.ShowDownloadDialog(this._packageToExtract);

            await this.DownloadRequested?.Invoke(this._packageToExtract);
        }
    }
}
