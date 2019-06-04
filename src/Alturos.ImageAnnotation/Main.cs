using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Contract.Amazon;
using Alturos.ImageAnnotation.Forms;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation
{
    public partial class Main : Form
    {
        private readonly IAnnotationPackageProvider _annotationPackageProvider;
        private readonly AnnotationConfig _annotationConfig;
        private bool _changedPackage;
        private AnnotationPackage _selectedPackage;

        public Main()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this._annotationPackageProvider = new AmazonAnnotationPackageProvider();

            this._annotationConfig = this._annotationPackageProvider.GetAnnotationConfigAsync().GetAwaiter().GetResult();
            if (this._annotationConfig == null)
            {
                this._annotationConfig = new AnnotationConfig();

                using (var configurationForm = new ConfigurationDialog())
                {
                    configurationForm.Setup(this._annotationConfig);
                    configurationForm.ShowDialog();
                }
            }

            this.InitializeComponent();
            this.downloadControl.Dock = DockStyle.Fill;

            this.annotationPackageListControl.Setup(this._annotationPackageProvider);
            this.annotationImageListControl.Setup(this._annotationPackageProvider);

            this.autoplaceAnnotationsToolStripMenuItem.Checked = true;

            this.annotationDrawControl.AutoplaceAnnotations = true;
            this.annotationDrawControl.SetObjectClasses(this._annotationConfig.ObjectClasses);
            this.annotationDrawControl.SetLabelsVisible(true);

            this.showLabelsToolStripMenuItem.Checked = true;
        }

        #region Form Events

        private void Main_Load(object sender, EventArgs e)
        {
            Task.Run(async () => await this.LoadPackagesAsync());
            this.RegisterEvents();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var confirmClosing = this.ConfirmDiscardingUnsavedChanges();
            if (!confirmClosing)
            {
                e.Cancel = true;
            }
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.UnregisterEvents();
        }

        #endregion

        #region Custom Control linking

        private void RegisterEvents()
        {
            this.annotationPackageListControl.PackageSelected += this.PackageSelected;

            this.annotationImageListControl.ImageSelected += this.ImageSelected;
            this.downloadControl.DownloadRequested += this.DownloadRequestedAsync;

            this.annotationDrawControl.ImageEdited += this.ImageEdited;

            this.tagListControl.TagsRequested += this.TagsRequested;

            this.KeyDown += this.annotationDrawControl.OnKeyDown;
        }

        private void UnregisterEvents()
        {
            this.annotationPackageListControl.PackageSelected -= this.PackageSelected;

            this.annotationImageListControl.ImageSelected -= this.ImageSelected;
            this.downloadControl.DownloadRequested -= this.DownloadRequestedAsync;

            this.annotationDrawControl.ImageEdited -= this.ImageEdited;

            this.tagListControl.TagsRequested -= this.TagsRequested;

            this.KeyDown -= this.annotationDrawControl.OnKeyDown;
        }

        #endregion

        #region Closing Dialog

        private bool ConfirmDiscardingUnsavedChanges()
        {
            var unsyncedPackages = this.annotationPackageListControl.GetAllPackages().Where(o => o.IsDirty);
            if (unsyncedPackages.Any())
            {
                using (var dialog = new CloseConfirmationDialog())
                {
                    dialog.StartPosition = FormStartPosition.CenterParent;

                    var dialogResult = dialog.ShowDialog(this);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #endregion

        #region Main Menu

        private void EnableMainMenu(bool enable)
        {
            this.Invoke((MethodInvoker)delegate
            {
                var showDownloadControl = enable;
                if (this._selectedPackage != null)
                {
                    showDownloadControl = enable && !this._selectedPackage.AvailableLocally;
                }

                this.menuStripMain.Enabled = enable;
                this.annotationPackageListControl.Enabled = enable;
                this.annotationImageListControl.Visible = enable;
                this.annotationDrawControl.Visible = enable;
                this.downloadControl.Visible = showDownloadControl;
                this.tagListControl.Visible = enable;
            });
        }

        private async void RefreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await this.LoadPackagesAsync();
        }

        private void SyncToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var packages = this.annotationPackageListControl.GetAllPackages().Where(o => o.IsDirty).ToArray();
            if (packages.Length > 0)
            {
                // Proceed with syncing
                var dialog = new SyncConfirmationDialog();
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.SetUnsyncedPackages(packages.ToList());

                var dialogResult = dialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    using (var syncDialog = new SyncProgressDialog(this._annotationPackageProvider))
                    {
                        syncDialog.Show(this);

                        _ = Task.Run(() => syncDialog.Sync(packages)).ContinueWith(o=>
                        {
                            dialog.Dispose();
                        });

                        this.annotationPackageListControl.RefreshData();
                    }
                }
            }
            else
            {
                MessageBox.Show("There are no unchanged packages to sync.", "Nothing to sync!");
            }
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new ExportDialog(this._annotationPackageProvider))
            {
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ShowDialog(this);
            }
        }

        private async void AddPackageStripMenuItem_Click(object sender, EventArgs e)
        {
            var uploadDialog = new UploadDialog(this._annotationPackageProvider);
            uploadDialog.StartPosition = FormStartPosition.CenterParent;

            var dialogResult = uploadDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                await this.LoadPackagesAsync();
            }
        }

        private void AutoplaceAnnotationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.autoplaceAnnotationsToolStripMenuItem.Checked = !this.autoplaceAnnotationsToolStripMenuItem.Checked;
            this.annotationDrawControl.AutoplaceAnnotations = this.autoplaceAnnotationsToolStripMenuItem.Checked;
        }

        private void ShowLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.showLabelsToolStripMenuItem.Checked = !this.showLabelsToolStripMenuItem.Checked;
            this.annotationDrawControl.SetLabelsVisible(this.showLabelsToolStripMenuItem.Checked);
        }

        private async void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new ConfigurationDialog())
            {
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.Setup(this._annotationConfig);
                var dialogResult = dialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    await this._annotationPackageProvider.SetAnnotationConfigAsync(this._annotationConfig);
                }
            }
        }

        private void HelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new HelpDialog())
            {
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ShowDialog(this);
            }
        }

        #endregion

        #region Logic

        private async Task LoadPackagesAsync()
        {
            this.EnableMainMenu(false);
            await this.annotationPackageListControl.LoadPackagesAsync();
            this.EnableMainMenu(true);
        }

        private void PackageSelected(AnnotationPackage package)
        {
            this._changedPackage = true;
            this._selectedPackage = package;

            this.annotationImageListControl.Hide();
            this.downloadControl.Hide();

            this.annotationImageListControl.Reset();
            this.annotationDrawControl.Reset();

            if (package != null)
            {
                this.tagListControl.SetTags(package);

                if (package.AvailableLocally)
                {
                    this.annotationImageListControl.SetPackage(package);
                    this.annotationImageListControl.Show();
                    this.annotationImageListControl.Focus();

                    this.annotationPackageListControl.RefreshData();
                }
                else
                {
                    this.downloadControl.ShowDownloadDialog(package);
                }
            }

            this._changedPackage = false;
        }

        private void ImageSelected(AnnotationImage image)
        {
            this.annotationDrawControl.SetImage(image);

            // Failsafe, because ImageSelected is triggered when the package is changed. We don't want to select the image in this case.
            if (this._changedPackage)
            {
                return;
            }

            if (image == null)
            {
                return;
            }

            this.annotationDrawControl.ApplyCachedBoundingBox();

            //if (image.BoundingBoxes == null)
            //{
            //    image.BoundingBoxes = new List<AnnotationBoundingBox>();
            //    this.ImageEdited(image);
            //}
        }

        private void ImageEdited(AnnotationImage annotationImage)
        {
            if (annotationImage == null)
            {
                return;
            }

            annotationImage.Package.IsDirty = true;

            annotationImage.Package.UpdateAnnotationStatus(annotationImage);
            this.annotationPackageListControl.RefreshData();
        }

        private async Task DownloadRequestedAsync(AnnotationPackage package)
        {
            await this._annotationPackageProvider.DownloadPackageAsync(package);

            if (this._selectedPackage == package)
            {
                this.PackageSelected(package);
            }
        }

        private List<string> TagsRequested()
        {
            //var form = new TagSelectionDialog();
            //form.Setup(this._annotationConfig);
            //form.ShowDialog();

            //return form.SelectedTags;
            return null;
        }

        #endregion
    }
}
