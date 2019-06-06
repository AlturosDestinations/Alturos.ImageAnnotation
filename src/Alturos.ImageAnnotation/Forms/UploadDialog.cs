using Alturos.ImageAnnotation.Contract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WK.Libraries.BetterFolderBrowserNS;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class UploadDialog : Form
    {
        private readonly IAnnotationPackageProvider _annotationPackageProvider;

        private bool _uploading;
        private List<string> _packagePaths;
        private CancellationTokenSource _tokenSource;

        public UploadDialog(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;

            this.InitializeComponent();

            this.labelUploadProgress.Visible = false;
        }

        private void UploadDialog_Load(object sender, EventArgs e)
        {
            var config = this._annotationPackageProvider.GetAnnotationConfigAsync().GetAwaiter().GetResult();
            this.tagSelectionControl.Setup(config);
        }

        private void ButtonSelectFolders_Click(object sender, System.EventArgs e)
        {
            var folderBrowser = new BetterFolderBrowser
            {
                Multiselect = true,
                RootFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            var dialogResult = folderBrowser.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                this._packagePaths = folderBrowser.SelectedFolders.ToList();
                this.dataGridViewPackages.DataSource = folderBrowser.SelectedFolders.Select(o => new {
                    Name = o,
                    FileCount = Directory.GetFiles(o).Length,
                    Size = this.GetDirectorySize(o)
                }).ToList();
            }

            this.buttonUpload.Enabled = this._packagePaths?.Count > 0;
        }

        private float GetDirectorySize(string directory)
        {
            long bytes = 0;

            foreach (var file in Directory.GetFiles(directory))
            {
                var fileInfo = new FileInfo(file);
                bytes += fileInfo.Length;
            }

            return (float)bytes / 1024 / 1024;
        }

        private async void ButtonUpload_Click(object sender, EventArgs e)
        {
            this.labelUploadProgress.Enabled = true;

            this.buttonSelectFolders.Enabled = false;
            this.buttonUpload.Enabled = false;
            this.tagSelectionControl.Enabled = false;

            this.buttonCancel.Enabled = true;

            await this.Upload();

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public async Task Upload()
        {
            this._uploading = true;

            _ = Task.Run(() => this.UpdateProgressBar());

            var tags = this.tagSelectionControl.SelectedTags.Select(o => o.Value).ToList();

            this._tokenSource = new CancellationTokenSource();
            var token = this._tokenSource.Token;

            try
            {
                await this._annotationPackageProvider.UploadPackagesAsync(this._packagePaths, tags, Environment.UserName, token);
            }
            catch (Exception)
            {
                MessageBox.Show("The upload was cancelled.", "Upload failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this._uploading = false;
        }

        private async Task UpdateProgressBar()
        {
            this.labelUploadProgress.Invoke((MethodInvoker)delegate
            {
                this.labelUploadProgress.Visible = true;
            });

            while (this._uploading)
            {
                var progress = this._annotationPackageProvider.GetUploadProgress();
                var percentageDone = progress.GetPercentDone();
                if (!double.IsNaN(percentageDone))
                {
                    this.labelUploadProgress.Invoke((MethodInvoker)delegate {
                        this.labelUploadProgress.Text = $"Upload in progress {progress.UploadedFiles}/{progress.FileCount} ({(int)percentageDone}%) - {Path.GetFileName(progress.CurrentFile)}";
                    });
                    this.progressBar.Invoke((MethodInvoker)delegate { this.progressBar.Value = (int)percentageDone; });
                }

                await Task.Delay(100);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void UploadDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            this._tokenSource?.Cancel();
        }
    }
}
