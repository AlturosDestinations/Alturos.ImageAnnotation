using System.Collections.Generic;

namespace Alturos.ImageAnnotation.Model.YoloConfig
{
    public class YoloConfig
    {
        public List<YoloConfigElement> YoloConfigElements { get; set; }

        public YoloConfig()
        {
            this.YoloConfigElements = new List<YoloConfigElement>();
        }
    }
}
