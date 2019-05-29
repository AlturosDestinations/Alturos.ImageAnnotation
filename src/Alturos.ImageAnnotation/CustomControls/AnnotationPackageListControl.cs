﻿using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Model;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class AnnotationPackageListControl : UserControl
    {
        private static ILog Log = LogManager.GetLogger(typeof(AnnotationPackageListControl));

        public event Action<AnnotationPackage> PackageSelected;

        private IAnnotationPackageProvider _annotationPackageProvider;

        public AnnotationPackageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
            this.labelLoading.Location = new Point(5, 20);
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
            var items = new List<AnnotationPackage>();

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                var package = row.DataBoundItem as AnnotationPackage;
                items.Add(package);
            }

            return items.ToArray();
        }

        public async Task LoadPackagesAsync()
        {
            try
            {
                this.groupBox1.Invoke((MethodInvoker)delegate
                {
                    this.groupBox1.Text = $"Packages";
                });

                this.dataGridView1.Invoke((MethodInvoker)delegate { this.dataGridView1.Visible = false; });

                this.labelLoading.Invoke((MethodInvoker)delegate { this.labelLoading.Visible = true; });
                var packages = await this._annotationPackageProvider.GetPackagesAsync(false);
                this.labelLoading.Invoke((MethodInvoker)delegate { this.labelLoading.Visible = false; });

                if (packages?.Length > 0)
                {
                    this.dataGridView1.Invoke((MethodInvoker)delegate
                    {
                        this.dataGridView1.Visible = true;
                        this.dataGridView1.DataSource = packages;
                    });

                    this.groupBox1.Invoke((MethodInvoker)delegate
                    {
                        this.groupBox1.Text = $"Packages ({packages.Length})";
                    });
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Error on loading packages", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var package = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationPackage;
            this.PackageSelected?.Invoke(package);
        }

        private void DownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var packages = new List<AnnotationPackage>();
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                packages.Add(row.DataBoundItem as AnnotationPackage);
            }

            packages.ForEach(o => Task.Run(() => this.DownloadPackage(o, false)));
        }

        private void RedownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var packages = new List<AnnotationPackage>();
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                packages.Add(row.DataBoundItem as AnnotationPackage);
            }

            packages.ForEach(o => Task.Run(() => this.DownloadPackage(o, true)));
        }

        private async Task DownloadPackage(AnnotationPackage package, bool redownload)
        {
            if (package.Downloading)
            {
                return;
            }

            if (package.AvailableLocally && !redownload)
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

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
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
    }
}
