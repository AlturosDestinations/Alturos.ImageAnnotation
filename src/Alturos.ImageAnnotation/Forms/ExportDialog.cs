using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Helper;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class ExportDialog : Form
    {
        private readonly IAnnotationPackageProvider _annotationPackageProvider;
        private readonly AnnotationConfig _config;

        private IAnnotationExportProvider _annotationExportProvider;
        private CancellationTokenSource _tokenSource;
        private bool _downloading;
        private AnnotationPackageTransferProgress _downloadProgress;
        private AnnotationPackage _downloadedPackage;

        public ExportDialog(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;
            this.InitializeComponent();

            this.labelDownloadProgress.Visible = false;

            this._config = this._annotationPackageProvider.GetAnnotationConfigAsync().GetAwaiter().GetResult();
            this.dataGridViewTags.DataSource = this._config.Tags;
            this.dataGridViewObjectClasses.DataSource = this._config.ObjectClasses.ToList();

            // Set export providers
            var objects = InterfaceHelper.GetImplementations<IAnnotationExportProvider>();

            this.comboBoxExportProvider.DataSource = objects;
            this.comboBoxExportProvider.DisplayMember = "Name";

            // Make data grid views not create their own columns
            this.dataGridViewTags.AutoGenerateColumns = false;
            this.dataGridViewResult.AutoGenerateColumns = false;
        }

        private async void ButtonSearch_Click(object sender, EventArgs e)
        {
            var tags = this.dataGridViewTags.SelectedRows.Cast<DataGridViewRow>().Select(o => o.DataBoundItem as AnnotationPackageTag);

            this.Invoke((MethodInvoker)delegate { this.EnableExportMenu(false); });

            var items = await this._annotationPackageProvider.GetPackagesAsync(tags?.ToArray());

            var objectClasses = this.GetSelectedObjectClasses();
            var packages = items.Where(o => o.IsAnnotated && o.Images.Any(p => p.BoundingBoxes.Any(q => objectClasses.Select(t => t.Id).Contains(q.ObjectIndex)))).ToList();

            this.dataGridViewResult.DataSource = packages;
            this.labelPackageCount.Text = $"{packages.Count.ToString()} found";

            this.Invoke((MethodInvoker)delegate { this.EnableExportMenu(true); });
        }

        private ObjectClass[] GetSelectedObjectClasses()
        {
            var objectClasses = new List<ObjectClass>();

            foreach (DataGridViewRow row in this.dataGridViewObjectClasses.Rows)
            {
                if (Convert.ToBoolean(row.Cells[0].Value) == true)
                {
                    objectClasses.Add(row.DataBoundItem as ObjectClass);
                }
            }

            return objectClasses.ToArray();
        }

        private async void ButtonExport_Click(object sender, EventArgs e)
        {
            this.buttonCancel.Enabled = true;

            await this.Export();
            this.Close();
        }

        private async Task Export()
        {
            this.EnableExportMenu(false);

            var packages = this.dataGridViewResult.DataSource as List<AnnotationPackage>;
            if (packages == null)
            {
                return;
            }

            // Download missing packages
            var successful = await this.DownloadMissingPackages(packages.Where(o => !o.AvailableLocally).ToList());
            if (!successful)
            {
                return;
            }

            // Create folders
            var rootPath = "exports";
            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            var path = Path.Combine(rootPath, DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss"));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Export
            this._annotationExportProvider.Export(path, packages.ToArray(), this.GetSelectedObjectClasses(), this.trackBarTrainingPercentage.Value);

            this.EnableExportMenu(true);

            // Open folder
            Process.Start(path);
        }

        private async Task<bool> DownloadMissingPackages(List<AnnotationPackage> packages)
        {
            var successful = true;

            this._downloading = true;

            this._downloadProgress = new AnnotationPackageTransferProgress
            {
                FileCount = packages.Count
            };

            _ = Task.Run(() => this.UpdateProgressBar());

            this._tokenSource = new CancellationTokenSource();
            var token = this._tokenSource.Token;

            try
            {
                for (var i = 0; i < packages.Count; i++)
                {
                    this._downloadedPackage = packages[i];
                    this._downloadProgress.CurrentFile = packages[i].PackageName;

                    packages[i].Downloading = true;
                    packages[i] = await this._annotationPackageProvider.DownloadPackageAsync(packages[i], token);

                    this._downloadProgress.TransferedFiles++;
                    this._downloadProgress.CurrentFilePercentDone = 0;

                    this.dataGridViewResult.Refresh();
                }
            }
            catch (Exception)
            {
                successful = false;
                MessageBox.Show("The download was cancelled.", "Download failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this._downloading = false;

            return successful;
        }

        private async Task UpdateProgressBar()
        {
            this.labelDownloadProgress.Invoke((MethodInvoker)delegate
            {
                this.labelDownloadProgress.Visible = true;
            });

            while (this._downloading)
            {
                var progress = this._downloadProgress;
                progress.CurrentFilePercentDone = (int)this._downloadedPackage.DownloadProgress;

                var percentageDone = progress.GetPercentDone();

                if (!double.IsNaN(percentageDone))
                {
                    this.labelDownloadProgress.Invoke((MethodInvoker)delegate {
                        this.labelDownloadProgress.Text = $"Download in progress {(int)percentageDone}% (Package {progress.TransferedFiles}/{progress.FileCount} {this._downloadProgress.CurrentFile})";
                    });
                    this.progressBar.Invoke((MethodInvoker)delegate { this.progressBar.Value = (int)percentageDone; });
                }

                await Task.Delay(100);
            }
        }

        private void EnableExportMenu(bool enable)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.comboBoxExportProvider.Enabled = enable;
                this.buttonSearch.Enabled = enable;
                this.buttonExport.Enabled = enable;
            });
        }

        private void ComboBoxExportProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            var obj = ((NameValueObject)this.comboBoxExportProvider.SelectedItem).Value;
            this._annotationExportProvider = (IAnnotationExportProvider)Activator.CreateInstance((Type)obj);
            this._annotationExportProvider?.Setup(this._config);
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExportDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._tokenSource?.Cancel();
        }

        private void TrackBarTrainingPercentage_Scroll(object sender, EventArgs e)
        {
            this.labelTrainingPercentage.Text = $"{this.trackBarTrainingPercentage.Value}%";
        }

        private void TrackBarImageSize_Scroll(object sender, EventArgs e)
        {
            this.trackBarImageSize.Value = (int)(Math.Round(this.trackBarImageSize.Value / 32.0) * 32);
            this.labelImageSize.Text = $"{this.trackBarImageSize.Value}";
        }
    }
}
