namespace Alturos.ImageAnnotation.Model.YoloConfig
{
    public class Maxpool : YoloConfigElement
    {
        public int Stride { get; set; } = 1;
        public int Size { get; set; } = 1;
    }
}
