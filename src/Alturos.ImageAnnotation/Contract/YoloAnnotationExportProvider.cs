using Alturos.ImageAnnotation.CustomControls;
using Alturos.ImageAnnotation.Helper;
using Alturos.ImageAnnotation.Model;
using Alturos.ImageAnnotation.Model.YoloConfig;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Contract
{
    [Description("Yolo")]
    public class YoloAnnotationExportProvider : IAnnotationExportProvider
    {
        public Control Control { get; set; }

        private const string DataFolderName = "data";
        private const string BackupFolderName = "backup";
        private const string ImageFolderName = "obj";
        private const string YoloConfigPath = @"..\..\Resources\yolov3.cfg";
        private const string TinyYoloConfigPath = @"..\..\Resources\yolov3-tiny_obj.cfg";

        private AnnotationConfig _config;
        private Dictionary<AnnotationImage, string> _exportedNames;

        public YoloAnnotationExportProvider()
        {
            this.Control = new YoloExportControl();
        }

        public void Setup(AnnotationConfig config)
        {
            this._config = config;
        }

        public void Export(string path, AnnotationPackage[] packages, ObjectClass[] objectClasses)
        {
            // Create folders
            var dataPath = Path.Combine(path, DataFolderName);
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            var backupPath = Path.Combine(path, BackupFolderName);
            if (!Directory.Exists(backupPath))
            {
                Directory.CreateDirectory(backupPath);
            }

            var imagePath = Path.Combine(dataPath, ImageFolderName);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            // Split images randomly into two lists
            // Training list contains the images Yolo uses for training. "_trainingPercentage" dictates how many percent of all images are used for this.
            // Testing list contains all remaining images that Yolo uses to validate how well it performs based on the training data.
            // Unannotated images are not taken into account and will not be exported.

            var images = new List<AnnotationImage>();
            var trainingImages = new List<AnnotationImage>();
            var testingImages = new List<AnnotationImage>();

            var yoloControl = this.Control as YoloExportControl;

            foreach (var package in packages)
            {
                var availableImages = package.Images.Where(o => o.BoundingBoxes != null && o.BoundingBoxes.Count != 0).ToList();
                availableImages.RemoveAll(o => !o.BoundingBoxes.Any(p => objectClasses.Select(q => q.Id).Contains(p.ObjectIndex)));

                var rng = new Random();
                var shuffledImages = availableImages.OrderBy(o => rng.Next()).ToList();

                var count = (int)(shuffledImages.Count * (yoloControl.TrainingPercentage / 100.0));
                trainingImages.AddRange(shuffledImages.Take(count));
                testingImages.AddRange(shuffledImages.Skip(count));

                images.AddRange(availableImages);
            }

            this._exportedNames = new Dictionary<AnnotationImage, string>();
            for (var i = 0; i < images.Count; i++)
            {
                var image = images[i];
                var newName = $"export{i.ToString("D5")}{Path.GetExtension(image.ImageName)}";
                this._exportedNames[image] = newName;
            }

            this.CreateFiles(dataPath, imagePath, images.ToArray(), objectClasses);
            this.CreateMetaData(dataPath, trainingImages.ToArray(), testingImages.ToArray(), objectClasses);

            var yoloConfigPath = yoloControl.UseTinyYoloConfig ? TinyYoloConfigPath : YoloConfigPath;
            this.CreateYoloConfig(path, yoloConfigPath, objectClasses);
            this.CreateCommandFile(path);
        }

        private void CreateFiles(string dataPath, string imagePath, AnnotationImage[] images, ObjectClass[] objectClasses)
        {
            var stringBuilderDict = new Dictionary<int, StringBuilder>();
            foreach (var objectClass in objectClasses)
            {
                stringBuilderDict[objectClass.Id] = new StringBuilder();
            }

            var packagesFolder = ConfigurationManager.AppSettings["extractionFolder"];

            var mappingsFile = "mappings.txt";
            var imageMappingSb = new StringBuilder();

            foreach (var image in images)
            {
                imageMappingSb.AppendLine($"{this._exportedNames[image]} {Path.Combine(image.Package.PackageName, image.ImageName)}");

                var newFilePath = Path.Combine(imagePath, this._exportedNames[image]);

                for (var j = 0; j < image.BoundingBoxes.Count; j++)
                {
                    if (image.BoundingBoxes[j] != null)
                    {
                        stringBuilderDict[image.BoundingBoxes[j].ObjectIndex].AppendLine(Path.GetFullPath(newFilePath));
                    }
                }

                // Copy image
                File.Copy(Path.Combine(packagesFolder, image.Package.PackageName, image.ImageName), newFilePath, true);

                // Create bounding boxes
                this.CreateBoundingBoxes(image.BoundingBoxes, Path.ChangeExtension(newFilePath, "txt"), objectClasses);
            }

            // Create mappings file
            File.WriteAllText(Path.Combine(dataPath, mappingsFile), imageMappingSb.ToString());
        }

        /// <summary>
        /// Writes the bounding boxes to a file
        /// </summary>
        private void CreateBoundingBoxes(List<AnnotationBoundingBox> boundingBoxes, string filePath, ObjectClass[] objectClasses)
        {
            var sb = new StringBuilder();
            foreach (var box in boundingBoxes)
            {
                if (!objectClasses.Select(o => o.Id).Contains(box.ObjectIndex))
                {
                    continue;
                }
                
                sb.Append(box.ObjectIndex).Append(" ");
                sb.Append(box.CenterX.ToString("0.0000", CultureInfo.InvariantCulture)).Append(" ");
                sb.Append(box.CenterY.ToString("0.0000", CultureInfo.InvariantCulture)).Append(" ");
                sb.Append(box.Width.ToString("0.0000", CultureInfo.InvariantCulture)).Append(" ");
                sb.Append(box.Height.ToString("0.0000", CultureInfo.InvariantCulture)).Append(" ");
                sb.AppendLine();
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        /// <summary>
        /// Creates the obj.names and obj.data files
        /// </summary>
        private void CreateMetaData(string dataPath, AnnotationImage[] trainingImages, AnnotationImage[] testingImages, ObjectClass[] objectClasses)
        {
            var namesFile = "obj.names";
            var dataFile = "obj.data";
            var trainFile = "train.txt";
            var testFile = "test.txt";

            var objectNames = objectClasses.Select(o => o.Name).ToArray();
            var sb = new StringBuilder();

            // Create obj.names
            foreach (var name in objectNames)
            {
                sb.AppendLine(name);
            }
            File.WriteAllText(Path.Combine(dataPath, $"{namesFile}"), sb.ToString());

            // Create obj.data
            sb.Clear();
            sb.AppendLine($"classes = {objectNames.Length}");
            sb.AppendLine($"train = {DataFolderName}/{trainFile}");
            sb.AppendLine($"valid = {DataFolderName}/{testFile}");
            sb.AppendLine($"names = {DataFolderName}/{namesFile}");
            sb.AppendLine("backup = backup/");
            File.WriteAllText(Path.Combine(dataPath, $"{dataFile}"), sb.ToString());

            // Create training file
            sb.Clear();
            foreach (var image in trainingImages.OrderBy(o => this._exportedNames[o]))
            {
                sb.AppendLine($"{DataFolderName}/{ImageFolderName}/{this._exportedNames[image]}");
            }
            File.WriteAllText(Path.Combine(dataPath, $"{trainFile}"), sb.ToString());

            // Create testing file
            sb.Clear();
            foreach (var image in testingImages.OrderBy(o => this._exportedNames[o]))
            {
                sb.AppendLine($"{DataFolderName}/{ImageFolderName}/{this._exportedNames[image]}");
            }
            File.WriteAllText(Path.Combine(dataPath, $"{testFile}"), sb.ToString());
        }

        private void CreateYoloConfig(string dataPath, string yoloConfigPath, ObjectClass[] objectClasses)
        {
            var fileName = "yolo-obj.cfg";

            var yoloConfig = YoloConfigParser.Parse(yoloConfigPath);

            var net = yoloConfig.YoloConfigElements.OfType<Net>().Single();
            net.Batch = 64;
            net.Subdivisions = 8;

            var imageSize = (this.Control as YoloExportControl).ImageSize;
            net.Width = imageSize;
            net.Height = imageSize;

            var maxBatches = objectClasses.Length * 2000;
            net.MaxBatches = maxBatches;
            net.Steps = new int[] { (int)(maxBatches * 0.8), (int)(maxBatches * 0.9) };
            
            foreach (var yolo in yoloConfig.YoloConfigElements.OfType<Yolo>())
            {
                yolo.Classes = objectClasses.Length;

                var convolutionalIndex = yoloConfig.YoloConfigElements.IndexOf(yolo) - 1;
                var convolutional = yoloConfig.YoloConfigElements[convolutionalIndex] as Convolutional;

                convolutional.Filters = (objectClasses.Length + 5) * 3;
            }

            var composedConfig = YoloConfigParser.Compose(yoloConfig);
            File.WriteAllText(Path.Combine(dataPath, fileName), composedConfig);
        }

        private void CreateCommandFile(string path)
        {
            File.WriteAllText(Path.Combine(path, "start_training.cmd"), "darknet.exe detector train data/obj.data yolo-obj.cfg darknet53.conv.74");
        }
    }
}
