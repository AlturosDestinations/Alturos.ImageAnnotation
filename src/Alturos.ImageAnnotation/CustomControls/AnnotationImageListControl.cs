using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class AnnotationImageListControl : UserControl
    {
        public event Action<AnnotationImage> ImageSelected;

        private IAnnotationPackageProvider _annotationPackageProvider;
        private List<AnnotationImage> _annotationImages;
        private BindingSource _bindingSource;

        public AnnotationImageListControl()
        {
            this.InitializeComponent();
            this.dataGridView1.AutoGenerateColumns = false;

            this._bindingSource = new BindingSource();
            this.dataGridView1.DataSource = this._bindingSource;
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
            this._annotationImages = null;
            this._bindingSource.ResetBindings(false);
        }

        public void SetPackage(AnnotationPackage package)
        {
            this._annotationImages = package.Images;
            this._bindingSource.DataSource = this._annotationImages;
            this._bindingSource.ResetBindings(false);
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var image = this.dataGridView1.CurrentRow?.DataBoundItem as AnnotationImage;
            if (image == null)
            {
                return;
            }

            this.ImageSelected?.Invoke(image);
        }

        private void DataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            try
            {
                if (this.dataGridView1.Rows.Count <= e.RowIndex)
                {
                    return;
                }

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
            catch (Exception exception)
            {

            }
        }

        private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you want to delete the selected image(s)?", "Confirm deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            var successful = true;
            var failedImages = new List<AnnotationImage>();

            var deleteImages = this.dataGridView1.SelectedRows.Cast<DataGridViewRow>().Select(o => o.DataBoundItem as AnnotationImage).ToList();
            this.dataGridView1.ClearSelection();

            foreach (var image in deleteImages)
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

            this._bindingSource.ResetBindings(false);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
