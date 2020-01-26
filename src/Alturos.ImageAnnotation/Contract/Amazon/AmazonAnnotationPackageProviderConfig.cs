namespace Alturos.ImageAnnotation.Contract.Amazon
{
    public class AmazonAnnotationPackageProviderConfig
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string BucketName { get; set; }
        public string ExtractionFolder { get; set; }
        public string DbTableName { get; set; }
        public string S3ServiceUrl { get; set; }
        public string DynamoDbServiceUrl { get; set; }
}
}
