using Amazon.DynamoDBv2.DataModel;

namespace Alturos.ImageAnnotation.Contract.Amazon
{
    [DynamoDBTable("ImageAnnotationPackageTag")]
    public class AnnotationTagDto
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        public string Tag { get; set; }
    }
}
