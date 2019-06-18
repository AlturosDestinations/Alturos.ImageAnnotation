using System;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.CustomControls
{
    public partial class YoloExportControl : UserControl
    {
        public int TrainingPercentage
        {
            get { return this.trackBarTrainingPercentage.Value; }
        }

        public int ImageSize
        {
            get { return this.trackBarImageSize.Value; }
        }

        public bool UseTinyYoloConfig
        {
            get { return this.checkBoxUseTinyConfig.Checked; }
        }

        public YoloExportControl()
        {
            this.InitializeComponent();
        }

        private void TrackBarTrainingPercentage_Scroll(object sender, EventArgs e)
        {
            this.labelTrainingPercentage.Text = $"{this.trackBarTrainingPercentage.Value}%";
        }

        private void TrackBarImageSize_Scroll(object sender, EventArgs e)
        {
            this.trackBarImageSize.Value = (int)(Math.Round(this.trackBarImageSize.Value / 32.0) * 32);
            this.labelImageSize.Text = $"{this.trackBarImageSize.Value}";
        }
    }
}
