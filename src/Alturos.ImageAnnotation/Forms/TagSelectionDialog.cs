using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class TagSelectionDialog : Form
    {
        private readonly AnnotationConfig _config;
        private readonly List<AnnotationPackageTag> _selectedTags;

        public List<AnnotationPackageTag> Tags { get; private set; }

        public TagSelectionDialog(AnnotationConfig config, List<AnnotationPackageTag> selectedTags)
        {
            this.InitializeComponent();

            this._config = config;
            this._selectedTags = selectedTags;
        }

        private void TagSelectionDialog_Load(object sender, EventArgs e)
        {
            this.tagSelectionControl.Setup(this._config);
            this.tagSelectionControl.SetSelectedTags(this._selectedTags);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Tags = this.tagSelectionControl.SelectedTags;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
