using System;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class CloseConfirmationDialog : Form
    {
        public CloseConfirmationDialog()
        {
            this.InitializeComponent();
        }

        private void ButtonDiscard_Click(object sender, EventArgs e)
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
