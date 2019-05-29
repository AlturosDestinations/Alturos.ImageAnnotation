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
        private const double _trainingPercentage = 70;
        private const string _dataFolderName = "data";
        private const string _imageFolderName = "obj";

        private AnnotationConfig _config;

        public void Setup(AnnotationConfig config)
        {
            this._config = config;
        }

        public void Export(string path, AnnotationPackage[] packages)
        {
            // Create folders
            var dataPath = Path.Combine(path, _dataFolderName);
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            var imagePath = Path.Combine(dataPath, _imageFolderName);
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

            var count = (int)(shuffledImages.Count * (_trainingPercentage / 100));
            var trainingImages = shuffledImages.Take(count);
            var testingImages = shuffledImages.Skip(count);

            this.CreateFiles(dataPath, imagePath, images.ToArray());
            this.CreateMetaData(dataPath, trainingImages.ToArray(), testingImages.ToArray());
        }

        private void CreateFiles(string dataPath, string imagePath, AnnotationImage[] images)
        {
            var stringBuilderDict = new Dictionary<int, StringBuilder>();
            foreach (var objectClass in this._config.ObjectClasses)
            {
                stringBuilderDict[objectClass.Id] = new StringBuilder();
            }

            var usedFileNames = new List<string>();
            var packagesFolder = ConfigurationManager.AppSettings["extractionFolder"];

            foreach (var image in images)
            {
                var newFileName = Path.GetFileName(image.ImagePath);
                while (usedFileNames.Contains(newFileName))
                {
                    newFileName = Path.GetFileNameWithoutExtension(image.ImagePath) + "(1)" + Path.GetExtension(image.ImagePath);
                }

                usedFileNames.Add(newFileName);

                var newFilePath = Path.Combine(imagePath, newFileName);

                for (var i = 0; i < image.BoundingBoxes.Count; i++)
                {
                    if (image.BoundingBoxes[i] != null)
                    {
                        stringBuilderDict[image.BoundingBoxes[i].ObjectIndex].AppendLine(Path.GetFullPath(newFilePath));
                    }
                }

                // Copy image
                File.Copy(Path.Combine(packagesFolder, image.Package.PackageName, image.ImageName), newFilePath, true);

                // Create bounding boxes
                this.CreateBoundingBoxes(image.BoundingBoxes, Path.ChangeExtension(newFilePath, "txt"));
            }
        }

        /// <summary>
        /// Writes the bounding boxes to a file
        /// </summary>
        private void CreateBoundingBoxes(List<AnnotationBoundingBox> boundingBoxes, string filePath)
        {
            var sb = new StringBuilder();
            foreach (var box in boundingBoxes)
            {
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
        private void CreateMetaData(string dataPath, AnnotationImage[] trainingImages, AnnotationImage[] testingImages)
        {
            var namesFile = "obj.names";
            var dataFile = "obj.data";
            var trainFile = "train.txt";
            var testFile = "test.txt";

            var objectNames = this._config.ObjectClasses.Select(o => o.Name).ToArray();
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
            sb.AppendLine($"train = {_dataFolderName}/{trainFile}");
            sb.AppendLine($"valid = {_dataFolderName}/{testFile}");
            sb.AppendLine($"names = {_dataFolderName}/{namesFile}");
            sb.AppendLine("backup = backup/");
            File.WriteAllText(Path.Combine(dataPath, $"{dataFile}"), sb.ToString());

            // Create training file
            sb.Clear();
            foreach (var image in trainingImages)
            {
                sb.AppendLine($"{_dataFolderName}/{_imageFolderName}/{image.ImageName}");
            }
            File.WriteAllText(Path.Combine(dataPath, $"{trainFile}"), sb.ToString());

            // Create testing file
            sb.Clear();
            foreach (var image in testingImages)
            {
                sb.AppendLine($"{_dataFolderName}/{_imageFolderName}/{image.ImageName}");
            }
            File.WriteAllText(Path.Combine(dataPath, $"{testFile}"), sb.ToString());
        }
    }
}
