using Alturos.ImageAnnotation.Forms;
using Alturos.ImageAnnotation.Model;
using System.Linq;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class TagEditControl : UserControl
    {
        private AnnotationConfig _annotationConfig;
        private AnnotationPackage _package;

        public TagEditControl()
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

            this.tagDisplayControl.SetTags(this._package.Tags?.ToArray());
        }

        private void ButtonEdit_Click(object sender, System.EventArgs e)
        {
            var dialog = new TagSelectionDialog(this._annotationConfig, this.tagDisplayControl.Tags?.Select(o => new AnnotationPackageTag { Value = o }).ToList());

            dialog.ShowDialog();
            if (dialog.DialogResult == DialogResult.OK)
            {
                this._package.Tags = dialog.Tags.Select(o => o.Value).ToList();
                this._package.IsDirty = true;
            }

            this.tagDisplayControl.SetTags(this._package.Tags?.ToArray());
        }
    }
}
