![Alturos.ImageAnnotation](doc/logo-banner.png)

# Alturos.ImageAnnotation

The purpose of this project is to manage training data for Neural Networks. The images are stored in an object storage for example Amazon S3.
In our case we have image data for different runs that we want to annotate together. You can upload a folder into a package.
For every package you can set your own tags... this information is stored in a database for example Amazon DynamoDB.

![object detection result](/doc/AlturosImageAnnotation.png)

## Features

 - Collaborative annotation of images
 - Verification of image annotation data
 - Export for yolo (train.txt, test.txt, obj.names) with filters
 - No requirement for a custom server

## Installation

You have the choice to use AWS or install your own environment.

- [Cloud Installation](doc/CLOUD_INSTALLATION.md)
- [Local Installation](doc/LOCAL_INSTALLATION.md)

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

### Extract images from a video

If you have a video file and need the individual frames you can use [ffmpeg](https://ffmpeg.org) to extract the images. This command exports every 10th frame in the video.
`ffmpeg -i input.mp4 -vf "select=not(mod(n\,10))" -vsync vfr 1_every_10/img_%03d.jpg`

## Articles of interest

- [Training YOLOv3 : Deep Learning based Custom Object Detector](https://www.learnopencv.com/training-yolov3-deep-learning-based-custom-object-detector/)

## Credits

This program uses icons from the Silk icon set created by Mark James, which can be found [here](http://www.famfamfam.com/lab/icons/silk/).
The icon set is licensed under a [CC BY 3.0 license](https://creativecommons.org/licenses/by/3.0/). Some changes were made to the icons.

## Other Image Annotation Tools

- [rectlabel.com](https://rectlabel.com)
