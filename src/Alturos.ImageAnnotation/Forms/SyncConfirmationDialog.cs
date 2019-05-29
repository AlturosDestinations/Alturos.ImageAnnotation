using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class SyncConfirmationDialog : Form
    {
        public SyncConfirmationDialog()
        {
            this.InitializeComponent();

            this.dataGridView.AutoGenerateColumns = false;
        }

        public void SetDescriptions(string upperText, string lowerText)
        {
            this.labelUpperDescription.Text = upperText;
            this.labelLowerDescription.Text = lowerText;
        }

        public void SetButtonName(string okButton)
        {
            this.buttonOK.Text = okButton;
        }

        public void SetUnsyncedPackages(List<AnnotationPackage> packages)
        {
            this.dataGridView.DataSource = packages;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
