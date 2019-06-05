using Alturos.ImageAnnotation.Contract;
using Alturos.ImageAnnotation.Forms;
using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class TagListControl : UserControl
    {
        public event Func<List<string>> TagsRequested;

        private AnnotationConfig _annotationConfig;
        private AnnotationPackage _package;

        public TagListControl()
        {
            this.InitializeComponent();
        }

        public void SetConfig(AnnotationConfig config)
        {
            this._annotationConfig = config;
        }

        public void SetTags(AnnotationPackage package)
        {
            if (package == null)
            {
                return;
            }

            this._package = package;

            this.RefreshDatagrid();
        }

        private void ButtonAdd_Click(object sender, System.EventArgs e)
        {
            var dialog = new TagSelectionDialog(this._annotationConfig, this.dataGridView1.DataSource as List<AnnotationPackageTag>);

            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                this._package.Tags = dialog.Tags.Select(o => o.Value).ToList();
                this._package.IsDirty = true;
            }

            this.RefreshDatagrid();
        }

        private void RemoveTagToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = this.dataGridView1.Rows[this.dataGridView1.CurrentCell.RowIndex];
            var tag = row.DataBoundItem as AnnotationPackageTag;

            this._package.Tags?.Remove(tag.Value);

            this.RefreshDatagrid();
        }

        private void RefreshDatagrid()
        {
            var items = this._package.Tags?.Select(o => new AnnotationPackageTag { Value = o });
            this.dataGridView1.DataSource = items?.ToList();
        }
    }
}
