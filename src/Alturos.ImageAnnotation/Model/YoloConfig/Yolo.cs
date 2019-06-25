namespace Alturos.ImageAnnotation.Model.YoloConfig
{
    public class Yolo : YoloConfigElement
    {
        public int[] Mask { get; set; }
        public int[][] Anchors { get; set; }
        public int Classes { get; set; } = 20;
        public int Num { get; set; } = 1;
        public float Jitter { get; set; } = 0.2f;
        public float IgnoreThresh { get; set; } = 0.5f;
        public float TruthThresh { get; set; } = 1;
        public int Random { get; set; } = 0;
    }
}
