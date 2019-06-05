using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace Alturos.ImageAnnotation.Contract.Amazon
{
    public class AnnotationPackageDto
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string User { get; set; }
        public bool IsAnnotated { get; set; }
        public double AnnotationPercentage { get; set; }
        public List<AnnotationImageDto> Images { get; set; }
        public List<string> Tags { get; set; }

        public override bool Equals(object obj)
        {
            if (this == null || obj == null)
            {
                return false;
            }

            return this.Id == (obj as AnnotationPackageDto).Id;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
