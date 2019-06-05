using Alturos.ImageAnnotation.Helper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Alturos.ImageAnnotation.Model
{
    public class AnnotationPackage
    {
        public string ExternalId { get; set; }
        public string User { get; set; }

        public string PackageName { get; set; }
        public long TotalBytes { get; set; }

        public bool Downloading { get; set; }
        public bool Enqueued { get; set; }
        public bool AvailableLocally { get; set; }
        public double DownloadProgress { get; set; }
        public long TransferredBytes { get; set; }

        public bool IsDirty { get; set; }

        public bool IsAnnotated { get; set; }
        public double AnnotationPercentage { get; set; }
        public List<AnnotationImage> Images { get; set; }
        public List<string> Tags { get; set; }

        private FileInfo[] _files;

        public void PrepareImages(string packagePath)
        {
            if (this.Images == null)
            {
                this.Images = new List<AnnotationImage>();
            }

            if (this._files == null)
            {
                var allowedExtensions = new[] { ".png", ".jpg", ".bmp" };
                this._files = Directory.GetFiles(packagePath)
                    .Where(file => allowedExtensions.Any(file.ToLower().EndsWith))
                    .Select(o => new FileInfo(o))
                    .OrderBy(o => o.Name.GetFirstNumber())
                    .ToArray();
            }

            var query = from file in this._files
                        join image in this.Images on file.Name equals image.ImageName into j1
                        from annotationImage in j1.DefaultIfEmpty(new AnnotationImage())
                        select new AnnotationImage
                        {
                            BoundingBoxes = annotationImage?.BoundingBoxes,
                            ImageName = file.Name,
                            ImagePath = file.FullName,
                            Package = this
                        };

            this.Images = query.ToList();
        }

        public void UpdateAnnotationStatus(AnnotationImage annotationImage)
        {
            if (!this.Images.Contains(annotationImage))
            {
                this.Images.Add(annotationImage);
            }
            
            var annotationPercentage = this.Images.Count(o => o.BoundingBoxes != null) / (double)this.Images.Count * 100.0;

            this.AnnotationPercentage = annotationPercentage;
            this.IsAnnotated = annotationPercentage >= 100;
        }

        public string DirtyPackageName
        {
            get
            {
                // An asterisk is commonly attached to filenames when they are dirty
                return $"{this.PackageName}{(this.IsDirty ? "*" : "")}";
            }
        }
    }
}
