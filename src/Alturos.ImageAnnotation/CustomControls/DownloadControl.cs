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

            this.labelNotification.Text = "The package is not available locally yet. Please download it first";
            this.progressBarDownload.Value = (int)package.DownloadProgress;
            this.labelPercentage.Text = string.Empty;
            this.labelDownload.Text = string.Empty;

            this._packageToExtract = package;

            this.Show();

            Task.Run(() => this.ShowDownloadProgress(package));
        }

        private async Task ShowDownloadProgress(AnnotationPackage package)
        {
            while (package.DownloadProgress < 100 && package.Downloading && this._packageToExtract == package)
            {
                this.labelNotification.Invoke((MethodInvoker)delegate
                {
                    if (this._packageToExtract == package)
                    {
                        this.labelNotification.Text = package.Enqueued ?
                        "Download queued... Wait for other downloads to finish first..." :
                        $"Downloading {package.PackageName}. Please wait...";
                    }
                });
                this.progressBarDownload.Invoke((MethodInvoker)delegate {
                    if (this._packageToExtract == package)
                    {
                        this.progressBarDownload.Value = (int)package.DownloadProgress;
                    }
                });

                this.labelPercentage.Invoke((MethodInvoker)delegate
                {
                    if (this._packageToExtract == package)
                    {
                        this.labelPercentage.Text = $"{(int)package.DownloadProgress}%";
                    }
                });
                this.labelDownload.Invoke((MethodInvoker)delegate
                {
                    if (this._packageToExtract == package)
                    {
                        this.labelDownload.Text = package.Enqueued ?
                            "" :
                            $"{package.TransferredBytes / 1024.0 / 1024.0:0.00} MB of {package.TotalBytes / 1024.0 / 1024.0:0.00} MB";
                    }
                });

                await Task.Delay(200);
            }
        }

        private async void ButtonDownload_Click(object sender, EventArgs e)
        {
            this._packageToExtract.Downloading = true;

            this.ShowDownloadDialog(this._packageToExtract);

            await this.DownloadRequested?.Invoke(this._packageToExtract);
        }
    }
}
