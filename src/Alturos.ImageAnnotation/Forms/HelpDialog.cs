using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Forms
{
    public partial class HelpDialog : Form
    {
        public HelpDialog()
        {
            this.InitializeComponent();
        }

        private void ButtonOk_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
