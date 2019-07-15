## Cloud Installation
AWS Preparation

AWS has a free tier for the first 12 months of S3 use (up to 5GB) and DynamoDB is free for up to 25GB. So it should be possible to use the tool for 12 months without cost, if you stick to the restrictions. Further information can be found here [AWS free tier](https://aws.amazon.com/de/free/)

1. Create an [AWS Account](https://portal.aws.amazon.com/billing/signup)
1. Create a DynamoDB Table with the name `ObjectDetectionImageAnnotation`, copy the Amazon Resource Name you find in the overview tab and replace the arn in the following policy. `arn:aws:dynamodb:eu-west-1:XXXXXXX:table/ObjectDetectionImageAnnotation`
1. Create a S3 Bucket and replace the `mys3bucketname` with a bucket name you'd like to use. See the [Amazon S3 Bucket Naming Requirements](https://docs.aws.amazon.com/awscloudtrail/latest/userguide/cloudtrail-s3-bucket-naming-requirements.html).
1. Create a new user for this tool in the IAM and add the policy to it below.
1. Change the bucketName, accessKeyId and the secretAccessKey in Alturos.ImageAnnotation.exe.config
1. Start the application

### AWS Policy
```
{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Effect": "Allow",
            "Action": [
                "s3:ListBucket"
            ],
            "Resource": [
                "arn:aws:s3:::mys3bucketname"
            ]
        },
        {
            "Effect": "Allow",
            "Action": [
                "s3:PutObject",
                "s3:GetObject",
                "s3:DeleteObject"
            ],
            "Resource": [
                "arn:aws:s3:::mys3bucketname/*"
            ]
        },
        {
            "Effect": "Allow",
            "Action": [
                "dynamodb:*"
            ],
            "Resource": [
                "arn:aws:dynamodb:eu-west-1:XXXXXXX:table/ObjectDetectionImageAnnotation"
            ]
        }
    ]
}
```

### App Config Setup

Once you have installed all the necessary components, you still need to adjust the `Alturos.ImageAnnotation.exe.config` file.

* Change `bucketName` to the bucket name you chose earlier to use instead of `mys3bucketname`.

* Change the `accessKeyId` and `secretAccessKey` to the keys AWS is using.
