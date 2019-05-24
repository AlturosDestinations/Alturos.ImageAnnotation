using System.Collections.Generic;
using System.IO;

namespace Alturos.ImageAnnotation.Model
{
    public class AnnotationImage
    {
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public List<AnnotationBoundingBox> BoundingBoxes { get; set; }
        public AnnotationPackage Package { get; set; }
    }
}
