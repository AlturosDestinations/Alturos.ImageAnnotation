namespace Alturos.ImageAnnotation.Model.YoloConfig
{
    public class Convolutional
    {
        public int Filters { get; set; } = 1;
        public int Size { get; set; } = 1;
        public int Stride { get; set; } = 1;
        public int Pad { get; set; } = 0;
        public string Activation { get; set; } = "logistic";
        public int BatchNormalize { get; set; } = 0;
    }
}
