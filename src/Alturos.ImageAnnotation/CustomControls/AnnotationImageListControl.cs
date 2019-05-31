using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class AnnotationImageListControl : UserControl
    {
        private List<AnnotationImage> _selectedImages;
        private IAnnotationPackageProvider _annotationPackageProvider;

        public event Action<AnnotationImage> ImageSelected;

        public AnnotationImageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;
        }

        public void Setup(IAnnotationPackageProvider annotationPackageProvider)
        {
            this._annotationPackageProvider = annotationPackageProvider;
        }

        public AnnotationImage[] GetAll()
        {
            var items = this.dataGridView1.DataSource as List<AnnotationImage>;
            return items.ToArray();
        }

        public void Reset()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.Refresh();
        }

        public void SetPackage(AnnotationPackage package)
        {
            this.dataGridView1.DataSource = package.Images;
            this.dataGridView1.Refresh();
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var image = this.dataGridView1.CurrentRow.DataBoundItem as AnnotationImage;
            if (image == null)
            {
                return;
            }

            this.ImageSelected?.Invoke(image);
        }

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            var item = this.dataGridView1.Rows[e.RowIndex].DataBoundItem as AnnotationImage;

            if (item == null)
            {
                return;
            }

            if (item.BoundingBoxes != null)
            {
                this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.GreenYellow;
                return;
            }

            this.dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
        }

        private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var successful = true;
            var failedImages = new List<AnnotationImage>();

            foreach (var image in this._selectedImages)
            {
                if (!await this._annotationPackageProvider.DeleteImageAsync(image))
                {
                    successful = false;
                    failedImages.Add(image);

                    continue;
                }

                image.Package.Images.Remove(image);
            }

            if (!successful)
            {
                var sb = new StringBuilder();
                failedImages.ForEach(o => sb.AppendLine(o.ImageName));
                MessageBox.Show("Couldn't delete the following images:\n\n" + sb.ToString(), "Deletion failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.dataGridView1.Refresh();
        }

        private void ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var images = new List<AnnotationImage>();
            foreach (DataGridViewRow row in this.dataGridView1.SelectedRows)
            {
                images.Add(row.DataBoundItem as AnnotationImage);
            }

            this._selectedImages = images;
        }
    }
}
