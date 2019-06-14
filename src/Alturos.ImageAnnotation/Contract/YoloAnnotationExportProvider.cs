using Alturos.ImageAnnotation.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Alturos.ImageAnnotation.Contract
{
    [Description("Yolo")]
    public class YoloAnnotationExportProvider : IAnnotationExportProvider
    {
        private const double TrainingPercentage = 70;
        private const string DataFolderName = "data";
        private const string ImageFolderName = "obj";
        private const string YoloConfigPath = @"..\..\Resources\yolov3.cfg";

        private AnnotationConfig _config;

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

            var imagePath = Path.Combine(dataPath, ImageFolderName);
            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            // Split images randomly into two lists
            // Training list contains the images Yolo uses for training. "_trainingPercentage" dictates how many percent of all images are used for this.
            // Testing list contains all remaining images that Yolo uses to validate how well it performs based on the training data.
            // Unannotated images are not taken into account and will not be exported.
            var images = packages.SelectMany(o => o.Images).Where(o => o.BoundingBoxes != null && o.BoundingBoxes.Count != 0);
            var rng = new Random();
            var shuffledImages = images.OrderBy(o => rng.Next()).ToList();

            var count = (int)(shuffledImages.Count * (TrainingPercentage / 100));
            var trainingImages = shuffledImages.Take(count);
            var testingImages = shuffledImages.Skip(count);

            this.CreateFiles(dataPath, imagePath, images.ToArray(), objectClasses);
            this.CreateMetaData(dataPath, trainingImages.ToArray(), testingImages.ToArray(), objectClasses);
            this.CreateYoloConfig(path, YoloConfigPath, objectClasses);
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

            for (var i = 0; i < images.Length; i++)
            {
                var image = images[i];
                var newFileName = $"export{i.ToString("D5")}{Path.GetExtension(image.ImageName)}";

                imageMappingSb.AppendLine($"{newFileName} {Path.Combine(image.Package.PackageName, image.ImageName)}");

                var newFilePath = Path.Combine(imagePath, newFileName);
                var hasObjectClass = false;

                for (var j = 0; j < image.BoundingBoxes.Count; j++)
                {
                    if (image.BoundingBoxes[j] != null && objectClasses.Select(o => o.Id).Contains(image.BoundingBoxes[j].ObjectIndex))
                    {
                        stringBuilderDict[image.BoundingBoxes[j].ObjectIndex].AppendLine(Path.GetFullPath(newFilePath));
                        hasObjectClass = true;
                    }
                }

                if (hasObjectClass)
                {
                    // Copy image
                    File.Copy(Path.Combine(packagesFolder, image.Package.PackageName, image.ImageName), newFilePath, true);

                    // Create bounding boxes
                    this.CreateBoundingBoxes(image.BoundingBoxes, Path.ChangeExtension(newFilePath, "txt"), objectClasses);
                }
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
                sb.Append(box.CenterX).Append(" ");
                sb.Append(box.CenterY).Append(" ");
                sb.Append(box.Width).Append(" ");
                sb.Append(box.Height).Append(" ");
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
            foreach (var image in trainingImages)
            {
                sb.AppendLine($"{DataFolderName}/{ImageFolderName}/{image.ImageName}");
            }
            File.WriteAllText(Path.Combine(dataPath, $"{trainFile}"), sb.ToString());

            // Create testing file
            sb.Clear();
            foreach (var image in testingImages)
            {
                sb.AppendLine($"{DataFolderName}/{ImageFolderName}/{image.ImageName}");
            }
            File.WriteAllText(Path.Combine(dataPath, $"{testFile}"), sb.ToString());
        }

        private void CreateYoloConfig(string dataPath, string yoloConfigPath, ObjectClass[] objectClasses)
        {
            var fileName = "yolo-obj.cfg";

            var lines = File.ReadAllLines(yoloConfigPath);

            var batchLineIndex = Array.FindIndex(lines, o => o.StartsWith("batch"));
            lines[batchLineIndex] = "batch=64";

            var subdivisionsLineIndex = Array.FindIndex(lines, o => o.StartsWith("subdivisions"));
            lines[subdivisionsLineIndex] = "subdivisions=8";

            var maxBatches = objectClasses.Length * 2000;
            var maxBatchesLineIndex = Array.FindIndex(lines, o => o.StartsWith("max_batches"));
            lines[maxBatchesLineIndex] = $"max_batches={maxBatches}";

            var steps1 = (int)(maxBatches * 0.8);
            var steps2 = (int)(maxBatches * 0.9);
            var stepsLineIndex = Array.FindIndex(lines, o => o.StartsWith("steps"));
            lines[stepsLineIndex] = $"steps={steps1},{steps2}";

            var classLines = lines.Where(o => o.StartsWith("classes"));
            foreach (var classLine in classLines)
            {
                lines[Array.IndexOf(lines, classLine)] = $"classes={objectClasses.Length}";
            }

            var filterIndices = new List<int>();
            for (var i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("[yolo]"))
                {
                    var filterIndex = i;
                    while (!lines[filterIndex].StartsWith("filters"))
                    {
                        filterIndex--;
                    }
                    filterIndices.Add(filterIndex);
                }
            }
            foreach (var filterIndex in filterIndices)
            {
                lines[filterIndex] = $"filters={(objectClasses.Length + 5) * 3}";
            }

            File.WriteAllLines(Path.Combine(dataPath, fileName), lines);
        }
    }
}
