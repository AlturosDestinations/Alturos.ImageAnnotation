using Alturos.ImageAnnotation.Model;
using System.Collections.Generic;

namespace Alturos.ImageAnnotation.Contract.Amazon
{
    public class AnnotationImageDto
    {
        public string ImageName { get; set; }
        public List<AnnotationBoundingBox> BoundingBoxes { get; set; }
    }
}
