using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Helper;
using Alturos.ImageAnnotation.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class AnnotationPackageListControl : UserControl
    {
        public event Action<AnnotationPackage> PackageSelected;
        public event Action<AnnotationCategory> CategorySelected;
        public event Action<bool> DirtyUpdated;

        private static ILog Log = LogManager.GetLogger(typeof(AnnotationPackageListControl));

        private IAnnotationPackageProvider _annotationPackageProvider;
        private List<AnnotationPackage> _annotationPackages;
        private List<AnnotationPackage> _selectedPackages;
        private BindingSource _bindingSource;
        private AnnotationCategory _selectedCategory;

        public AnnotationPackageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;

            this._bindingSource = new BindingSource();
            this.dataGridView1.DataSource = this._bindingSource;

            this.comboBoxCategory.DataSource = Enum.GetValues(typeof(AnnotationCategory));
            this.comboBoxCategory.SelectedIndex = 0;
        }

        public void Setup(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;
        }

        public void RefreshData()
        {
            this.dataGridView1.Refresh();
            this.DirtyUpdated?.Invoke(this._annotationPackages.Any(o => o.IsDirty));
        }

        public AnnotationPackage[] GetAllPackages()
        {
            return this._annotationPackages.ToArray();
        }

        public async Task LoadPackagesAsync(bool annotated)
        {
            var groupBoxName = "Packages";

            try
            {
                this.groupBox1.Invoke((MethodInvoker)delegate
                {
                    this.groupBox1.Text = groupBoxName;
                });

                this.SetLoading(true);

                var packages = await this._annotationPackageProvider.GetPackagesAsync(annotated);
                this._annotationPackages = packages.ToList();

                this.SetLoading(false);

                if (this._annotationPackages?.Count > 0)
                {
                    this.dataGridView1.Invoke((MethodInvoker)delegate
                    {
                        this.RefreshGridData();
                    });

                    this.groupBox1.Invoke((MethodInvoker)delegate
                    {
                        this.groupBox1.Text = $"{groupBoxName} ({this._annotationPackages.Count:n0})";
                    });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Error on loading packages", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetLoading(bool loading)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.labelLoading.Visible = loading;
                this.dataGridView1.Visible = !loading;

                this.comboBoxCategory.Enabled = !loading;
                this.textBoxSearch.Enabled = !loading;
            });
        }

        public int GetSelectedPackageCount()
        {
            return this.dataGridView1.SelectedRows.Count;
        }

        private void RefreshGridData()
        {
            if (string.IsNullOrEmpty(this.textBoxSearch.Text))
            {
                this._bindingSource.DataSource = this._annotationPackages;
                return;
            }
            
            var packages = this._annotationPackages.Where(o => o.PackageName.Contains(this.textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
            this._bindingSource.DataSource = packages;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var package = this.dataGridView1.CurrentRow?.DataBoundItem as AnnotationPackage;
            this.PackageSelected?.Invoke(package);
        }

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count <= e.RowIndex)
                {
                    return;
                }

                var item = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as AnnotationPackage;

                if (item.IsAnnotated)
                {
                    this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.GreenYellow;
                    return;
                }

                if (item.AvailableLocally)
                {
                    this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
                    return;
                }

                this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            catch (Exception exception)
            {

            }
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private void DownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._selectedPackages.ForEach(o => Task.Run(() => this.DownloadPackage(o)));
        }

        private async Task DownloadPackage(AnnotationPackage package)
        {
            if (package.Downloading)
            {
                return;
            }

            if (package.AvailableLocally)
            {
                return;
            }

            package.AvailableLocally = false;
            package.Downloading = true;
            package.DownloadProgress = 0;

            // Reset UI for this package
            this.Invoke((MethodInvoker)delegate { this.PackageSelected?.Invoke(package); });

            var downloadedPackage = await this._annotationPackageProvider.DownloadPackageAsync(package);

            // Refresh UI once download is complete
            this.Invoke((MethodInvoker)delegate { this.PackageSelected?.Invoke(downloadedPackage); });
        }

        private async void ResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ParentForm.Invoke((MethodInvoker)delegate
            {
                this.ParentForm.Enabled = false;
            });

            var remotePackages = await this._annotationPackageProvider.GetPackagesAsync(false);

            foreach (var package in this._selectedPackages)
            {
                var remotePackage = remotePackages.FirstOrDefault(o => o.ExternalId == package.ExternalId);

                package.IsDirty = false;
                package.Images = remotePackage.Images;
                package.User = remotePackage.User;
                package.IsAnnotated = remotePackage.IsAnnotated;
                package.AnnotationPercentage = remotePackage.AnnotationPercentage;
                package.Tags = remotePackage.Tags;

                package.PrepareImages();
            }

            this.ParentForm.Invoke((MethodInvoker)delegate
            {
                this.ParentForm.Enabled = true;
            });

            var selectedPackage = this.dataGridView1.CurrentRow?.DataBoundItem as AnnotationPackage;
            if (selectedPackage == null)
            {
                return;
            }

            this.PackageSelected?.Invoke(selectedPackage);
        }

        private void ClearAnnotationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var package in this._selectedPackages)
            {
                var markAsDirty = false;

                foreach (var image in package.Images)
                {
                    if (image.BoundingBoxes != null)
                    {
                        markAsDirty = true;
                        image.BoundingBoxes = null;

                        package.UpdateAnnotationStatus(image);
                    }
                }

                if (markAsDirty)
                {
                    package.IsDirty = true;
                }
            }

            this.RefreshData();
        }

        private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you want to delete the selected package(s)?", "Confirm deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }
                
            var successful = true;
            var failedPackages = new List<AnnotationPackage>();

            foreach (var package in this._selectedPackages)
            {
                if (!await this._annotationPackageProvider.DeletePackageAsync(package))
                {
                    successful = false;
                    failedPackages.Add(package);

                    continue;
                }

                this._annotationPackages.Remove(package);
            }

            if (!successful)
            {
                var sb = new StringBuilder();
                failedPackages.ForEach(o => sb.AppendLine(o.PackageName));
                MessageBox.Show("Couldn't delete the following packages:\n\n" + sb.ToString(), "Deletion failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.RefreshData();
        }

        private void ComboBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var newCategory = (AnnotationCategory)this.comboBoxCategory.SelectedItem;

            if (this._selectedCategory != newCategory)
            {
                this._selectedCategory = newCategory;
                this.CategorySelected?.Invoke(newCategory);
            }
        }

        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView1.SelectionChanged -= DataGridView1_SelectionChanged;
            this.RefreshGridData();
            this.dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var packages = this.dataGridView1.SelectedRows.Cast<DataGridViewRow>().Select(o => o.DataBoundItem as AnnotationPackage);

            #region Show Download Button

            if (packages.All(o => o.AvailableLocally))
            {
                this.downloadToolStripMenuItem.Visible = false;
                this.toolStripSeparator1.Visible = false;
            }
            else
            {
                this.downloadToolStripMenuItem.Visible = true;
                this.toolStripSeparator1.Visible = true;
            }

            #endregion

            this._selectedPackages = packages.ToList();
        }
    }
}
