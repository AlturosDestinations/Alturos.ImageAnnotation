using Alturos.ImageAnnotation.Model;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alturos.ImageAnnotation.Contract.Amazon
{
    [Description("Amazon")]
    public class AmazonAnnotationPackageProvider : IAnnotationPackageProvider, IDisposable
    {
        public bool IsSyncing { get; private set; }
        public bool IsUploading { get; private set; }

        private readonly IAmazonS3 _client;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _bucketName;
        private readonly string _extractionFolder;
        private readonly Queue<AnnotationPackage> _packagesToDownload;
        private readonly string _configHashKey = "AnnotationConfiguration";

        private int _packagesToSync;
        private int _syncedPackages;
        private int _uploadedFiles;
        private double _filesToUpload;
        private AnnotationPackage _downloadedPackage;

        public AmazonAnnotationPackageProvider()
        {
            var accessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            var secretAccessKey = ConfigurationManager.AppSettings["secretAccessKey"];

            this._bucketName = ConfigurationManager.AppSettings["bucketName"]?.ToLower();
            this._extractionFolder = ConfigurationManager.AppSettings["extractionFolder"];

            if (string.IsNullOrEmpty(accessKeyId))
            {
                throw new ConfigurationErrorsException("The accessKeyId has not been configured. Please set it in the App.config");
            }

            if (string.IsNullOrEmpty(secretAccessKey))
            {
                throw new ConfigurationErrorsException("The secretAccessKey has not been configured. Please set it in the App.config");
            }

            if (string.IsNullOrEmpty(this._bucketName))
            {
                throw new ConfigurationErrorsException("The bucketName has not been configured. Please set it in the App.config");
            }

            this._client = new AmazonS3Client(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1);
            this._dynamoDbClient = new AmazonDynamoDBClient(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1);

            this._packagesToDownload = new Queue<AnnotationPackage>();
        }

        public void Dispose()
        {
            this._client?.Dispose();
            this._dynamoDbClient?.Dispose();
        }

        public async Task SetAnnotationConfigAsync(AnnotationConfig config)
        {
            try
            {
                var annotationConfig = new AnnotationConfigDto
                {
                    Id = this._configHashKey,
                    ObjectClasses = config.ObjectClasses,
                    Tags = config.Tags.Select(o => o.Value).ToList()
                };

                using (var context = new DynamoDBContext(this._dynamoDbClient))
                {
                    await context.SaveAsync(annotationConfig).ConfigureAwait(false);
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task<AnnotationConfig> GetAnnotationConfigAsync()
        {
            try
            {
                using (var context = new DynamoDBContext(this._dynamoDbClient))
                {
                    var annotationConfig = await context.LoadAsync<AnnotationConfigDto>(this._configHashKey).ConfigureAwait(false);
                    return new AnnotationConfig
                    {
                        ObjectClasses = annotationConfig.ObjectClasses,
                        Tags = annotationConfig.Tags.Select(o => new Model.AnnotationPackageTag { Value = o }).ToList()
                    };
                }
            }
            catch (Exception)
            {
                return await Task.FromResult<AnnotationConfig>(null);
            }
        }

        public async Task<AnnotationPackage[]> GetPackagesAsync(AnnotationPackageTag[] tags)
        {
            var scanConditions = new List<ScanCondition>();
            foreach (var tag in tags)
            {
                scanConditions.Add(new ScanCondition("Tags", ScanOperator.Contains, tag.Value));
            }

            return await this.GetPackagesAsync(scanConditions.ToArray()).ConfigureAwait(false);
        }

        public async Task<AnnotationPackage[]> GetPackagesAsync(bool annotated)
        {
            var scanConditions = new ScanCondition[]
            {
                new ScanCondition("IsAnnotated", ScanOperator.Equal, annotated)
            };

            return await this.GetPackagesAsync(scanConditions).ConfigureAwait(false);
        }

        private async Task<AnnotationPackage[]> GetPackagesAsync(ScanCondition[] scanConditions)
        {
            // Retrieve unannotated metadata
            using (var context = new DynamoDBContext(this._dynamoDbClient))
            {
                try
                {
                    var packageInfos = context.ScanAsync<AnnotationPackageDto>(scanConditions);

                    // Create packages
                    var retrievedPackages = await packageInfos.GetNextSetAsync().ConfigureAwait(false);

                    //var packages = retrievedPackages.Where(o => o.Id == "S-1-16142028098304285338.zip").Select(o => new AnnotationPackage
                    var packages = retrievedPackages.Select(o => new AnnotationPackage
                    {
                        ExternalId = o.Id,
                        PackageName = Path.GetFileNameWithoutExtension(o.Id),
                        AvailableLocally = false,
                        IsAnnotated = o.IsAnnotated,
                        AnnotationPercentage = o.AnnotationPercentage,
                        Tags = o.Tags,
                        Images = o.Images?.Select(x => new AnnotationImage
                        {
                            ImageName = x.ImageName,
                            BoundingBoxes = x.BoundingBoxes
                        }).ToList()
                    }).ToList();

                    // Get local folder if the package was already downloaded
                    foreach (var package in packages)
                    {
                        var path = Path.Combine(this._extractionFolder, package.PackageName);
                        if (Directory.Exists(path))
                        {
                            package.AvailableLocally = true;
                            package.PrepareImages(path);
                        }
                    }

                    //TODO: Put that thing in its own table...
                    packages.RemoveAll(o => o.PackageName == "AnnotationConfiguration");

                    return packages.OrderBy(o => o.ExternalId).ToArray();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        public async Task<AnnotationPackage> DownloadPackageAsync(AnnotationPackage package)
        {
            this._packagesToDownload.Enqueue(package);

            while (this._packagesToDownload.Peek() != package)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }

            this._downloadedPackage = package;

            if (!Directory.Exists(this._extractionFolder))
            {
                Directory.CreateDirectory(this._extractionFolder);
            }

            var files = await this._client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = this._bucketName,
                Prefix = package.PackageName
            });

            var request = new TransferUtilityDownloadDirectoryRequest
            {
                BucketName = this._bucketName,
                S3Directory = package.PackageName,
                LocalDirectory = Path.Combine(this._extractionFolder, package.PackageName)
            };

            request.DownloadedDirectoryProgressEvent += this.DownloadedDirectoryProgressEvent;

            using (var fileTransferUtility = new TransferUtility(this._client))
            {
                await fileTransferUtility.DownloadDirectoryAsync(request).ConfigureAwait(false);
            }

            request.DownloadedDirectoryProgressEvent -= this.DownloadedDirectoryProgressEvent;

            package.Downloading = false;
            package.AvailableLocally = true;

            var path = Path.Combine(this._extractionFolder, package.PackageName);
            package.PrepareImages(path);

            this._packagesToDownload.Dequeue();

            return package;
        }

        private void DownloadedDirectoryProgressEvent(object sender, DownloadDirectoryProgressArgs e)
        {
            if (this._downloadedPackage == null)
            {
                return;
            }

            this._downloadedPackage.TotalBytes = e.TotalBytes;
            this._downloadedPackage.TransferredBytes = e.TransferredBytes;
            this._downloadedPackage.DownloadProgress = (e.TransferredBytes / (double)e.TotalBytes) * 100;
        }

        public async Task UploadPackagesAsync(List<string> packagePaths, List<string> tags)
        {
            this.IsUploading = true;

            this._uploadedFiles = 0;

            var tasks = new List<Task>();
            foreach (var packagePath in packagePaths)
            {
                this._filesToUpload += Directory.GetFiles(packagePath).Count();
                tasks.Add(Task.Run(() => UploadPackageAsync(packagePath, tags)));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            this.IsUploading = false;
        }

        private async Task UploadPackageAsync(string packagePath, List<string> tags)
        {
            var packageName = Path.GetFileName(packagePath);
            await this.SyncPackagesAsync(new AnnotationPackage[]
            {
                new AnnotationPackage
                {
                    ExternalId = packageName,
                    PackageName = packageName,
                    IsAnnotated = false,
                    AnnotationPercentage = 0,
                    Images = new List<AnnotationImage>(),
                    Tags = tags
                }
            }).ConfigureAwait(false);

            var tasks = new List<Task>();

            var files = Directory.GetFiles(packagePath);
            foreach (var file in files)
            {
                tasks.Add(this.UploadFileAsync(file));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task UploadFileAsync(string filePath)
        {
            using (var fileTransferUtility = new TransferUtility(this._client))
            {
                var keyName = $"{Directory.GetParent(filePath).Name}/{Path.GetFileName(filePath)}";
                await fileTransferUtility.UploadAsync(filePath, "alturos.imageannotation", keyName).ConfigureAwait(false);
            }

            this._uploadedFiles++;
        }

        public async Task SyncPackagesAsync(AnnotationPackage[] packages)
        {
            this.IsSyncing = true;

            this._packagesToSync = packages.Length;
            this._syncedPackages = 0;

            var tasks = new List<Task>();
            foreach (var package in packages)
            {
                tasks.Add(Task.Run(() => this.SyncPackageAsync(package).ConfigureAwait(false)));
            }

            await Task.WhenAll(tasks).ConfigureAwait(false);

            this.IsSyncing = false;
        }

        private async Task<bool> SyncPackageAsync(AnnotationPackage package)
        {
            var info = new AnnotationPackageDto
            {
                Id = package.ExternalId,
                IsAnnotated = package.IsAnnotated,
                AnnotationPercentage = package.AnnotationPercentage,
                Tags = package.Tags
            };

            info.Images = new List<AnnotationImageDto>();
            if (package.Images != null)
            {
                foreach (var image in package.Images.Where(o => o.BoundingBoxes != null))
                {
                    info.Images.Add(new AnnotationImageDto
                    {
                        ImageName = image.ImageName,
                        BoundingBoxes = image.BoundingBoxes
                    });
                }
            }

            try
            {
                using (var context = new DynamoDBContext(this._dynamoDbClient))
                {
                    await context.SaveAsync(info).ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {

            }

            this._syncedPackages++;
            return true;
        }

        public async Task<bool> DeletePackageAsync(AnnotationPackage package)
        {
            try
            {
                var successful = true;

                // Delete images on S3
                foreach (var image in package.Images)
                {
                    if (!await this.DeleteImageAsync(image).ConfigureAwait(false))
                    {
                        successful = false;
                    }
                }

                // Delete package from DynamoDB
                using (var context = new DynamoDBContext(this._dynamoDbClient))
                {
                    await context.DeleteAsync(new AnnotationPackageDto
                    {
                        Id = package.ExternalId
                    }).ConfigureAwait(false);
                }

                // Delete local folder
                Directory.Delete(Path.Combine(this._extractionFolder, package.PackageName), true);

                return successful;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteImageAsync(AnnotationImage image)
        {
            try
            {
                // Delete image from S3
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = this._bucketName,
                    Key = $"{image.Package.PackageName}/{image.ImageName}"
                };

                var response = await this._client.DeleteObjectAsync(deleteObjectRequest).ConfigureAwait(false);

                // Delete image from DynamoDB
                using (var context = new DynamoDBContext(this._dynamoDbClient))
                {
                    var package = await context.LoadAsync<AnnotationPackageDto>(image.Package.ExternalId).ConfigureAwait(false);
                    package.Images?.RemoveAll(o => o.ImageName.Equals(image.ImageName));
                    await context.SaveAsync(package).ConfigureAwait(false);
                }

                // Delete local image
                var localImagePath = Path.Combine(this._extractionFolder, image.Package.PackageName, image.ImageName);
                if (File.Exists(localImagePath))
                {
                    File.Delete(localImagePath);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public double GetUploadProgress()
        {
            if (!this.IsUploading)
            {
                return 0;
            }

            return this._uploadedFiles / (double)this._filesToUpload * 100;
        }

        public double GetSyncProgress()
        {
            if (!this.IsSyncing)
            {
                return 0;
            }

            return this._syncedPackages / (double)this._packagesToSync * 100;
        }
    }
}
