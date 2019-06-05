using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class TagSelectionControl : UserControl
    {
        public List<AnnotationPackageTag> SelectedTags { get; private set; }
        public List<AnnotationPackageTag> AvailableTags { get; private set; }

        private AnnotationConfig _config;
        private BindingSource _selectedTagsSource;
        private BindingSource _availableTagsSource;

        public TagSelectionControl()
        {
            this.SelectedTags = new List<AnnotationPackageTag>();
            this.AvailableTags = new List<AnnotationPackageTag>();

            this.InitializeComponent();
        }

        private void TagSelectionControl_Load(object sender, EventArgs e)
        {
            this._selectedTagsSource = new BindingSource();
            this._availableTagsSource = new BindingSource();
            this.dataGridViewSelectedTags.DataSource = this._selectedTagsSource;
            this.dataGridViewAvailableTags.DataSource = this._availableTagsSource;
        }

        public void Setup(AnnotationConfig config)
        {
            this._config = config;
            this.AvailableTags = this._config.Tags.ToList();
            this._availableTagsSource.DataSource = this.AvailableTags;
        }

        public void SetSelectedTags(List<AnnotationPackageTag> selectedTags)
        {
            this.SelectedTags = selectedTags;
            this.AvailableTags = this.AvailableTags.Where(o => !selectedTags.Select(p => p.Value).Contains(o.Value)).ToList();

            this.RefreshTags();
        }

        private void TextBoxFilter_TextChanged(object sender, EventArgs e)
        {
            this.UpdateAvailableTags();
        }

        private void RefreshTags()
        {
            this.UpdateAvailableTags();
            this._selectedTagsSource.DataSource = this.SelectedTags;

            this._selectedTagsSource.ResetBindings(false);
            this._availableTagsSource.ResetBindings(false);
        }

        private void UpdateAvailableTags()
        {
            var filter = this.textBoxFilter.Text;
            if (!string.IsNullOrEmpty(filter))
            {
                this._availableTagsSource.DataSource = this.AvailableTags.Where(o => o.Value.Contains(filter)).ToList();
            }
            else
            {
                this._availableTagsSource.DataSource = this.AvailableTags;
            }
        }

        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            var rowsSelected = this.dataGridViewAvailableTags.SelectedRows;
            foreach (DataGridViewRow row in rowsSelected)
            {
                var tag = row.DataBoundItem as AnnotationPackageTag;
                this.SelectedTags.Add(tag);
                this.AvailableTags.Remove(tag);
            }

            this.RefreshTags();
        }

        private void ButtonRemove_Click(object sender, EventArgs e)
        {
            var rowsSelected = this.dataGridViewSelectedTags.SelectedRows;
            foreach (DataGridViewRow row in rowsSelected)
            {
                var tag = row.DataBoundItem as AnnotationPackageTag;
                this.SelectedTags.Remove(tag);
                this.AvailableTags.Add(tag);
            }

            this.RefreshTags();
        }
    }
}
