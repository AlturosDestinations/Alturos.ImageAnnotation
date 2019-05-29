﻿using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Contract.Amazon;
using Alturos.ImageAnnotation.Forms;
using Alturos.ImageAnnotation.Model;
using Microsoft.WindowsAPICodePack.Dialogs;
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

            this.autoplaceAnnotationsToolStripMenuItem.Checked = true;

            this.annotationDrawControl.AutoplaceAnnotations = true;
            this.annotationDrawControl.SetObjectClasses(this._annotationConfig.ObjectClasses);
            this.annotationDrawControl.ShowLabels = true;

            this.showLabelsToolStripMenuItem.Checked = true;
        }

        #region Initialization and Closing

        private void Main_Load(object sender, EventArgs e)
        {
            Task.Run(async () => await this.LoadPackagesAsync());
            this.RegisterEvents();
        }

        private async Task LoadPackagesAsync()
        {
            this.EnableMainMenu(false);
            await this.annotationPackageListControl.LoadPackagesAsync();
            this.EnableMainMenu(true);
        }

        private void EnableMainMenu(bool enable)
        {
            this.Invoke((MethodInvoker)delegate {
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

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            var confirmClosing = this.ConfirmDiscardingUnsavedChanges();
            if (!confirmClosing)
            {
                e.Cancel = true;
            }
        }

        private bool ConfirmDiscardingUnsavedChanges()
        {
            var unsyncedPackages = this.annotationPackageListControl.GetAllPackages().Where(o => o.IsDirty);
            if (unsyncedPackages.Any())
            {
                using (var syncConfirmationDialog = new SyncConfirmationDialog())
                {
                    syncConfirmationDialog.Text = "Confirm closing";
                    syncConfirmationDialog.SetDescriptions("The following packages still have unsaved changes", "Do you want to close and discard these changes?");
                    syncConfirmationDialog.SetUnsyncedPackages(unsyncedPackages.ToList());

                    var dialogResult = syncConfirmationDialog.ShowDialog();

                    if (dialogResult == DialogResult.Cancel)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.UnregisterEvents();
        }

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

        #region Main Menu

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
                var syncConfirmationDialog = new SyncConfirmationDialog();
                syncConfirmationDialog.Text = "Confirm syncing";
                syncConfirmationDialog.SetDescriptions("Do you want to sync the following packages?", string.Empty);
                syncConfirmationDialog.SetUnsyncedPackages(packages.ToList());

                var dialogResult = syncConfirmationDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    using (var syncDialog = new SyncProgressDialog(this._annotationPackageProvider))
                    {
                        syncDialog.Show();

                        _ = Task.Run(() => syncDialog.Sync(packages));

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
            using (var exportDialog = new ExportDialog(this._annotationPackageProvider))
            {
                exportDialog.ShowDialog();
            }
        }

        private void AddPackageStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new CommonOpenFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                IsFolderPicker = true,
                Multiselect = true
            })
            using (var tagDialog = new TagSelectionDialog())
            {
                var dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == CommonFileDialogResult.Ok)
                {
                    tagDialog.Setup(this._annotationConfig);
                    var tagDialogResult = tagDialog.ShowDialog();
                    if (tagDialogResult == DialogResult.OK)
                    {
                        var uploadDialog = new UploadProgressDialog(this._annotationPackageProvider);
                        uploadDialog.Show();

                        _ = Task.Run(() => uploadDialog.Upload(openFileDialog.FileNames.ToList(), tagDialog.SelectedTags));
                    }
                }
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
            this.annotationDrawControl.ShowLabels = this.showLabelsToolStripMenuItem.Checked;
        }

        private async void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var configurationForm = new ConfigurationDialog())
            {
                configurationForm.Setup(this._annotationConfig);
                var dialogResult = configurationForm.ShowDialog();
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
                dialog.ShowDialog();
            }
        }

        #endregion

        #region Delegate Callbacks

        private void PackageSelected(AnnotationPackage package)
        {
            this._changedPackage = true;
            this._selectedPackage = package;

            this.annotationImageListControl.Hide();
            this.downloadControl.Hide();
            this.tagListControl.Hide();

            this.annotationImageListControl.Reset();
            this.annotationDrawControl.Reset();

            if (package != null)
            {
                this.tagListControl.SetTags(package);

                if (package.AvailableLocally)
                {
                    this.annotationImageListControl.SetPackage(package);
                    this.annotationImageListControl.Show();

                    this.annotationPackageListControl.RefreshData();

                    this.tagListControl.Show();
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

            if (image.BoundingBoxes == null)
            {
                image.BoundingBoxes = new List<AnnotationBoundingBox>();
                this.ImageEdited(image);
            }
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
            var downloadedPackage = await this._annotationPackageProvider.DownloadPackageAsync(package);

            // Select folder to apply the images after extraction
            this.PackageSelected(downloadedPackage);
        }

        private List<string> TagsRequested()
        {
            var form = new TagSelectionDialog();
            form.Setup(this._annotationConfig);
            form.ShowDialog();

            return form.SelectedTags;
        }

        #endregion
    }
}
