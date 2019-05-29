using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Helper;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class ExportDialog : Form
    {
        private readonly IAnnotationPackageProvider _annotationPackageProvider;
        private readonly AnnotationConfig _config;

        private IAnnotationExportProvider _annotationExportProvider;
        private bool _exporting;

        public ExportDialog(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;
            this.InitializeComponent();

            this._config = this._annotationPackageProvider.GetAnnotationConfigAsync().GetAwaiter().GetResult();
            this.dataGridViewTags.DataSource = this._config.Tags;

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

            var items = await this._annotationPackageProvider.GetPackagesAsync(tags.ToArray());
            this.dataGridViewResult.DataSource = items.ToList();
            this.labelPackageCount.Text = $"{items.Length.ToString()} found";

            foreach (DataGridViewRow row in this.dataGridViewResult.Rows)
            {
                if ((row.DataBoundItem as AnnotationPackage).AvailableLocally)
                {
                    row.Cells[1].Value = true;
                }
            }

            this.Invoke((MethodInvoker)delegate { this.EnableExportMenu(true); });
        }

        private async void ButtonExport_Click(object sender, EventArgs e)
        {
            await this.Export();
            this.Close();
        }

        private async Task Export()
        {
            this.EnableExportMenu(false);
            this._exporting = true;
            
            // Create folders
            var path = DateTime.Now.ToString("yyyy-MM-dd hh-mm-ss");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Download what's missing
            var packages = this.dataGridViewResult.DataSource as List<AnnotationPackage>;

            for (var i = 0; i < packages.Count; i++)
            {
                if (!packages[i].AvailableLocally)
                {
                    packages[i].Downloading = true;
                    this.downloadControl.ShowDownloadDialog(packages[i]);

                    packages[i] = await this._annotationPackageProvider.DownloadPackageAsync(packages[i]);

                    var row = this.dataGridViewResult.Rows.Cast<DataGridViewRow>().Single(o => (o.DataBoundItem as AnnotationPackage).ExternalId == packages[i].ExternalId);
                    row.Cells[1].Value = true;

                    this.downloadControl.Hide();
                }
            }

            // Export
            this._annotationExportProvider.Export(path, packages.ToArray());

            this.EnableExportMenu(true);
            this._exporting = false;

            // Open folder
            Process.Start(path);
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

        private void ExportDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._exporting)
            {
                e.Cancel = true;
            }
        }
    }
}
