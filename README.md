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
<kbd>W</kbd><kbd>A</kbd><kbd>S</kbd><kbd>D</kbd><br>+<kbd>Shift</kbd><br>+<kbd>Ctrl</kbd><br>+<kbd>Alt</kbd> | Move Bounding Box<br>Resize<br>Quick<br>Invert

## Data preperation

If you have a video file and need the individual frames you can use [ffmpeg](https://ffmpeg.org) to extract the images. This command exports every 10th frame in the video.
`ffmpeg -i input.mp4 -vf "select=not(mod(n\,10))" -vsync vfr 1_every_10/img_%03d.jpg`

## Articles of interest

- [Training YOLOv3 : Deep Learning based Custom Object Detector](https://www.learnopencv.com/training-yolov3-deep-learning-based-custom-object-detector/)

## Installation

- [Cloud Installation](doc/CLOUD_INSTALLATION.md)
- [Local Installation](doc/LOCAL_INSTALLATION.md)

### App Config Setup

Once you have installed all the necessary components, you still need to adjust the `Alturos.ImageAnnotation.exe.config` file.

* Change `bucketName` to the bucket name you wish to you use for your database. See the [Amazon S3 Bucket Naming Requirements](https://docs.aws.amazon.com/awscloudtrail/latest/userguide/cloudtrail-s3-bucket-naming-requirements.html).

* Change the `accessKeyId` and `secretAccessKey` to the keys AWS is using.
If you did a quick installation, the keys should be `AKIAIOSFODNN7EXAMPLE` and `wJalrXUtnFEMI/K7MDENG/bPxRfiCYEXAMPLEKEY` respectively.
If you did a manual installation, use the same keys you chose while configuring AWS earlier.

* Change the `s3ServiceUrl` to `http://localhost:9000`

* Change the `dynamoDbServiceUrl` to `http://localhost:8000`

### The setup is now complete. Launch the run script to use it.

Run `run_local_environment.ps1` using the Windows PowerShell in order to start MinIO and the local DynamoDB.
Once you launch the project you should be able to use your local database.

If it doesn't work, try checking `http://localhost:8000/shell/` in your browser to see if the DynamoDB is running.

## Credits

This program uses icons from the Silk icon set created by Mark James, which can be found [here](http://www.famfamfam.com/lab/icons/silk/).
The icon set is licensed under a [CC BY 3.0 license](https://creativecommons.org/licenses/by/3.0/). Some changes were made to the icons.

## Other Tools

- [rectlabel.com](https://rectlabel.com)
