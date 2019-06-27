namespace Alturos.ImageAnnotation.Model.YoloConfig
{
    public class Net : YoloConfigElement
    {
        public int Batch { get; set; } = 1;
        public int Subdivisions { get; set; } = 1;
        public int Width { get; set; } = 0;
        public int Height { get; set; } = 0;
        public int Channels { get; set; } = 0;
        public float Momentum { get; set; } = 0.9f;
        public float Decay { get; set; } = 0.0001f;
        public float Angle { get; set; } = 0;
        public float Saturation { get; set; } = 1;
        public float Exposure { get; set; } = 1;
        public float Hue { get; set; } = 0;


        public float LearningRate { get; set; } = 0.001f;
        public int BurnIn { get; set; } = 0;
        public int MaxBatches { get; set; } = 0;
        public int Policy { get; set; } = 0;
        public int[] Steps { get; set; }
        public float[] Scales { get; set; }
    }
}
