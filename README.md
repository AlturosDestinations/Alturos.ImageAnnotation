![Alturos.ImageAnnotation](doc/logo-banner.png)

# Alturos.ImageAnnotation

The purpose of this project is to manage training data for Neural Networks. The data is stored at a central location, for example Amazon S3.
In our case we have image data for different runs that we want to annotate together. Every run is stored in an AnnotationPackage.
For every AnnotationPackage you can set your own tags... this information is stored in the Amazon DynamoDB.

![object detection result](/doc/AlturosImageAnnotation.png)

## Features

 - Collaborative annotation of images
 - Verification of image annotation data
 - Export for yolo (train.txt, test.txt, obj.names) with filters
 - No requirement for a custom server

## Keyboard Shortcuts

Shortcut | Description | 
--- | --- |
<kbd>↓</kbd> | Next image |
<kbd>↑</kbd> | Previous image |
<kbd>→</kbd> | Next Object Class |
<kbd>←</kbd> | Previous Object Class |
<kbd>0</kbd>-<kbd>9</kbd> | Select Object Class |

## Data preperation

If you have a video file and need the individual frames you can use [ffmpeg](https://ffmpeg.org) to extract the images. This command exports every 10th frame in the video.
`ffmpeg -i input.mp4 -vf "select=not(mod(n\,10))" -vsync vfr 1_every_10/img_%03d.jpg`

## Articles of interest

- [Training YOLOv3 : Deep Learning based Custom Object Detector](https://www.learnopencv.com/training-yolov3-deep-learning-based-custom-object-detector/)

## AWS Preparation

AWS has a free tier for the first 12 months of S3 use (up to 5GB) and DynamoDB is free for up to 25GB. So it should be possible to use the tool for 12 months without cost, if you stick to the restrictions. Further information can be found here [AWS free tier](https://aws.amazon.com/de/free/)

1. Create an [AWS Account](https://portal.aws.amazon.com/billing/signup)
1. Create a DynamoDB Table with the name `ObjectDetectionImageAnnotation`, copy the Amazon Resource Name you find in the overview tab and replace the arn in the following policy. `arn:aws:dynamodb:eu-west-1:XXXXXXX:table/ObjectDetectionImageAnnotation`
1. Create a S3 Bucket and replace the `mys3bucketname` with the new bucketname.
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

## An alternative solution so you don't need Amazon AWS

You can also use [MinIO](https://github.com/minio/minio) instead of S3 and a [local dynamodb]( https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/DynamoDBLocal.html)

## Credits

This program uses icons from the Silk icon set created by Mark James, which can be found [here](http://www.famfamfam.com/lab/icons/silk/).
The icon set is licensed under a [CC BY 3.0 license](https://creativecommons.org/licenses/by/3.0/). Some changes were made to the icons.
