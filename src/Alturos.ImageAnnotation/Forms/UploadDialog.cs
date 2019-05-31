using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class UploadDialog : Form
    {
        private IAnnotationPackageProvider _annotationPackageProvider;
        private bool _uploading;

        public UploadDialog(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;

            this.InitializeComponent();
        }

        public async Task Upload(List<string> packagePaths, List<string> tags)
        {
            this._uploading = true;

            _ = Task.Run(() => this.UpdateProgressBar());

            await this._annotationPackageProvider.UploadPackagesAsync(packagePaths, tags);

            this._uploading = false;

            this.Invoke((MethodInvoker)delegate { this.Close(); });
        }

        private async Task UpdateProgressBar()
        {
            while (this._uploading)
            {
                var progress = this._annotationPackageProvider.GetUploadProgress();
                if (!double.IsNaN(progress))
                {
                    this.progressBar.Invoke((MethodInvoker)delegate { this.progressBar.Value = (int)progress; });
                }

                await Task.Delay(100);
            }
        }
    }
}
