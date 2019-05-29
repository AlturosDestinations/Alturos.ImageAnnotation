![Alturos.ImageAnnotation](doc/logo-banner.png)

# Alturos.ImageAnnotation

The purpose of this project is to manage training data for Neural Networks. The data is stored at a central location, for example Amazon S3.
In our case we have image data for different runs that we want to annotate together. Every run is stored in an AnnotationPackage.

For every AnnotationPackage we have some Metadata, Weather, Color, ... this informations we are store in a DynamoDB Table.

![object detection result](/doc/AlturosImageAnnotation.png)

## Requirements

 - [Amazon AWS Account](https://aws.amazon.com/) (S3, DynamoDB)
 
## Features

 - Image annotation together
 - Verify Image annotation data
 - Export for yolo (train.txt, test.txt, obj.names) with filters

## Articles of interest

- [Training YOLOv3 : Deep Learning based Custom Object Detector](https://www.learnopencv.com/training-yolov3-deep-learning-based-custom-object-detector/)

## AWS Preparation

AWS has a free tier for the first 12 months of S3 use (up to 5GB) and DynamoDB is up to 25GB free. So it should be possible to use the tool for 12 months without cost, if you stick to the restrictions. Further information can be found here [AWS free tier](https://aws.amazon.com/de/free/)

1. Create an AWS Account
1. Create a DynamoDB Table and copy the Amazon Resource Name you found it in the overview tab, replace the arn in the following policy.
1. Create a S3 Bucket and replace the mys3bucketname with the new bucketname.
1. Create an own user for this tool in the IAM and add the policy to the new user.
1. Change the bucketName, accessKeyId and the secretAccessKey in Alturos.ImageAnnotation.exe.config
1. Start the Application

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
                "s3:GetObject"
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

## Use this tool without AWS

You can also use [MinIO](https://github.com/minio/minio) instead of S3 and a [local dynamodb]( https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.html)
