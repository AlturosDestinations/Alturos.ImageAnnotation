using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private void DataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        private async void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialogResult = MessageBox.Show("Are you sure you want to delete the selected image(s)?", "Confirm deletion", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
            if (dialogResult == DialogResult.Cancel)
            {
                return;
            }

            var deleteImages = this.dataGridView1.SelectedRows.Cast<DataGridViewRow>().Select(o => o.DataBoundItem as AnnotationImage).ToList();
            this.dataGridView1.ClearSelection();

            var package = deleteImages.Select(o => o.Package).FirstOrDefault();
            if (package != null)
            {
                package.Images.RemoveAll(o => deleteImages.Contains(o));
            }

            this._bindingSource.ResetBindings(false);

            var deleteResult = await this.DeleteImagesAsync(deleteImages);
            if (!deleteResult.Successful)
            {
                var sb = new StringBuilder();
                deleteResult.FailedImages.ForEach(o => sb.AppendLine(o));
                MessageBox.Show("Couldn't delete the following images:\n\n" + sb.ToString(), "Deletion failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<DeleteResult> DeleteImagesAsync(IEnumerable<AnnotationImage> annotationImages)
        {
            var result = new DeleteResult();
            result.FailedImages = new List<string>();
            result.Successful = true;

            foreach (var image in annotationImages)
            {
                if (!await this._annotationPackageProvider.DeleteImageAsync(image))
                {
                    result.Successful = false;
                    result.FailedImages.Add(image.ImageName);

                    continue;
                }
            }

            return result;
        }
    }
}
