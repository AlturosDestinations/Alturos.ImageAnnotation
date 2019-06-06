using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Model;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class SyncProgressDialog : Form
    {
        private IAnnotationPackageProvider _annotationPackageProvider;
        private bool _syncing;

        public SyncProgressDialog(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;

            this.InitializeComponent();
        }

        public async Task Sync(AnnotationPackage[] packages)
        {
            this._syncing = true;

            try
            {
                _ = Task.Run(() => this.UpdateProgressBar());
                await this._annotationPackageProvider.SyncPackagesAsync(packages);

                foreach (var package in packages)
                {
                    package.IsDirty = false;
                    package.User = Environment.UserName;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show($"Syncing failed! It's likely your changes have not been saved.\n\n" +
                    $"{exception.GetType().ToString()}\n\n" +
                    $"{exception.Message}", "Syncing error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this._syncing = false;

            this.Invoke((MethodInvoker)delegate { this.Close(); });
        }

        private async Task UpdateProgressBar()
        {
            while (this._syncing)
            {
                var progress = this._annotationPackageProvider.GetSyncProgress().GetPercentDone();
                if (!double.IsNaN(progress))
                {
                    this.progressBar.Invoke((MethodInvoker)delegate { this.progressBar.Value = (int)progress; });
                }

                await Task.Delay(100);
            }
        }
    }
}
