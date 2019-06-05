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

        private static ILog Log = LogManager.GetLogger(typeof(AnnotationPackageListControl));

        private IAnnotationPackageProvider _annotationPackageProvider;
        private List<AnnotationPackage> _annotationPackages;
        private List<AnnotationPackage> _selectedPackages;
        private BindingSource _bindingSource;

        public AnnotationPackageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.labelLoading.Location = new Point(5, 20);

            this._bindingSource = new BindingSource();
            this.dataGridView1.DataSource = this._bindingSource;
        }

        public void Setup(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;
        }

        public void RefreshData()
        {
            this.dataGridView1.Refresh();
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

                this.textBoxSearch.Invoke((MethodInvoker)delegate { this.textBoxSearch.Visible = false; });
                this.dataGridView1.Invoke((MethodInvoker)delegate { this.dataGridView1.Visible = false; });

                this.labelLoading.Invoke((MethodInvoker)delegate { this.labelLoading.Visible = true; });
                this._annotationPackages = (await this._annotationPackageProvider.GetPackagesAsync(annotated)).ToList();
                this.labelLoading.Invoke((MethodInvoker)delegate { this.labelLoading.Visible = false; });

                if (this._annotationPackages?.Count > 0)
                {
                    this.textBoxSearch.Invoke((MethodInvoker)delegate { this.textBoxSearch.Visible = true; });

                    this.dataGridView1.Invoke((MethodInvoker)delegate
                    {
                        this.dataGridView1.Visible = true;
                        this._bindingSource.DataSource = this._annotationPackages;
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

        public int GetSelectedPackageCount()
        {
            return this.dataGridView1.SelectedRows.Count;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var package = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationPackage;
            this.PackageSelected?.Invoke(package);
        }

        private void DownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._selectedPackages.ForEach(o => Task.Run(() => this.DownloadPackage(o)));
        }

        private void RedownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var package in this._selectedPackages)
            {
                package.AvailableLocally = false;
                Task.Run(() => this.DownloadPackage(package));
            }
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

        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            this.dataGridView1.SelectionChanged -= DataGridView1_SelectionChanged;

            if (string.IsNullOrEmpty(this.textBoxSearch.Text))
            {
                this._bindingSource.DataSource = this._annotationPackages;
            }
            else
            {
                var packages = this._annotationPackages.Where(o => o.PackageName.Contains(this.textBoxSearch.Text, StringComparison.OrdinalIgnoreCase)).ToArray();
                this._bindingSource.DataSource = packages;
            }

            this.dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var packages = new List<AnnotationPackage>();
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                packages.Add(row.DataBoundItem as AnnotationPackage);
            }

            if (packages.All(o => o.AvailableLocally))
            {
                this.contextMenuStrip1.Items[0].Visible = false;
            }
            else
            {
                this.contextMenuStrip1.Items[0].Visible = true;
            }

            if (!packages.Any(o => o.AvailableLocally))
            {
                this.contextMenuStrip1.Items[1].Visible = false;
            }
            else
            {
                this.contextMenuStrip1.Items[1].Visible = true;
            }

            this._selectedPackages = packages;
        }

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
