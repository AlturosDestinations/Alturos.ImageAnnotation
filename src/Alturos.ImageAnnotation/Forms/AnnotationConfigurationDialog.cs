using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class AnnotationConfigurationDialog : Form
    {
        private AnnotationConfig _config;
        private BindingSource _bindingSourceObjectClasses;
        private BindingSource _bindingSourceTags;
        private string _cachedValue;

        public AnnotationConfigurationDialog()
        {
            this.InitializeComponent();

            this.dataGridViewObjectClasses.AutoGenerateColumns = false;
            this.dataGridViewTags.AutoGenerateColumns = false;
        }

        public void Setup(AnnotationConfig config)
        {
            this._config = config;

            this._bindingSourceObjectClasses = new BindingSource();
            this._bindingSourceObjectClasses.DataSource = config.ObjectClasses;
            this.dataGridViewObjectClasses.DataSource = this._bindingSourceObjectClasses;

            this._bindingSourceTags = new BindingSource();
            this._bindingSourceTags.DataSource = config.Tags;
            this.dataGridViewTags.DataSource = this._bindingSourceTags;
        }

        private void ButtonAddObjectClass_Click(object sender, EventArgs e)
        {
            var text = this.textBoxObjectClass.Text;
            if (!string.IsNullOrEmpty(text) && !this._config.ObjectClasses.Any(o => o.Name == text))
            {
                var objectClass = new ObjectClass()
                {
                    Id = this._config.ObjectClasses.Count,
                    Name = text
                };

                this._config.ObjectClasses.Add(objectClass);
            }

            this._bindingSourceObjectClasses.ResetBindings(false);
            this.dataGridViewObjectClasses.Refresh();

            this.textBoxObjectClass.Text = string.Empty;
            this.textBoxObjectClass.Focus();
        }

        private void ButtonAddTag_Click(object sender, EventArgs e)
        {
            var tag = this.textBoxTag.Text;
            if (!string.IsNullOrEmpty(tag) && !this._config.Tags.Any(o => o.Value.Equals(tag, StringComparison.OrdinalIgnoreCase)))
            {
                this._config.Tags.Add(new AnnotationPackageTag { Value = tag });
            }

            this._bindingSourceTags.ResetBindings(false);
            this.dataGridViewTags.Refresh();

            this.textBoxTag.Text = string.Empty;
            this.textBoxTag.Focus();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void RemoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tag = this.dataGridViewTags.CurrentRow.DataBoundItem as AnnotationPackageTag;

            this._config.Tags.Remove(tag);

            this._bindingSourceTags.ResetBindings(false);
            this.dataGridViewTags.Refresh();
        }

        private void DataGridViewTags_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.BeginCellEdit(this.dataGridViewTags, e);
        }

        private void DataGridViewTags_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.EndCellEdit(this.dataGridViewTags, e);
        }

        private void DataGridViewObjectClasses_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            this.BeginCellEdit(this.dataGridViewObjectClasses, e);
        }

        private void DataGridViewObjectClasses_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.EndCellEdit(this.dataGridViewObjectClasses, e);
        }

        private void BeginCellEdit(DataGridView dgv, DataGridViewCellCancelEventArgs e)
        {
            this._cachedValue = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        private void EndCellEdit(DataGridView dgv, DataGridViewCellEventArgs e)
        {
            var cell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (string.IsNullOrEmpty(cell.Value?.ToString()))
            {
                cell.Value = this._cachedValue;
            }
            else
            {
                var values = new List<string>();
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.Index == e.RowIndex)
                    {
                        continue;
                    }

                    values.Add(row.Cells[e.ColumnIndex].Value.ToString());
                }

                var number = 0;
                var originalValue = cell.Value.ToString();
                while (values.Any(o => o == cell.Value.ToString()))
                {
                    number++;
                    cell.Value = $"{originalValue}{number.ToString()}";
                }
            }
        }
    }
}
