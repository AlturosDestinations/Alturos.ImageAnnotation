using System.Collections.Generic;

namespace Alturos.ImageAnnotation.Model
{
    public class AnnotationConfig
    {
        public List<ObjectClass> ObjectClasses { get; set; }
        public List<AnnotationPackageTag> Tags { get; set; }

        public AnnotationConfig()
        {
            this.ObjectClasses = new List<ObjectClass>()
            {
                new ObjectClass { Id = 0, Name = "MyObject1" },
                new ObjectClass { Id = 1, Name = "MyObject2" }
            };
            this.Tags = new List<AnnotationPackageTag>()
            {
                new AnnotationPackageTag { Value = "Red" },
                new AnnotationPackageTag { Value = "Green" },
                new AnnotationPackageTag { Value = "Blue" },
            };
        }
    }
}
