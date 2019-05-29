using Alturos.ImageAnnotation.Model;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.S3;
using Amazon.S3.IO;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Alturos.ImageAnnotation.Contract.Amazon
{
    public class AmazonAnnotationPackageProvider : IAnnotationPackageProvider
    {
        public bool IsSyncing { get; set; }

        private readonly IAmazonS3 _client;
        private readonly IAmazonDynamoDB _dynamoDbClient;
        private readonly string _bucketName;
        private readonly string _extractionFolder;
        private readonly List<AnnotationPackage> _currentlyDownloadedPackages;
        private readonly string _configHashKey = "AnnotationConfiguration";

        private int _packagesToSync;
        private int _syncedPackages;

        public AmazonAnnotationPackageProvider()
        {
            var accessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            var secretAccessKey = ConfigurationManager.AppSettings["secretAccessKey"];

            this._bucketName = ConfigurationManager.AppSettings["bucketName"];
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

            this._currentlyDownloadedPackages = new List<AnnotationPackage>();
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

                    return packages.ToArray();
                }
                catch (Exception exception)
                {
                    throw exception;
                }
            }
        }

        public async Task<AnnotationPackage> DownloadPackageAsync(AnnotationPackage package)
        {
            if (!Directory.Exists(this._extractionFolder))
            {
                Directory.CreateDirectory(this._extractionFolder);
            }

            var files = await this._client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = this._bucketName,
                Prefix = package.PackageName
            });

            var request = new TransferUtilityDownloadDirectoryRequest();
            request.BucketName = this._bucketName;
            request.S3Directory = package.PackageName;
            request.LocalDirectory = Path.Combine(this._extractionFolder, package.PackageName);

            this._currentlyDownloadedPackages.Add(package);
            request.DownloadedDirectoryProgressEvent += this.DownloadedDirectoryProgressEvent;

            var fileTransferUtility = new TransferUtility(this._client);
            await fileTransferUtility.DownloadDirectoryAsync(request);

            this._currentlyDownloadedPackages.Remove(package);
            request.DownloadedDirectoryProgressEvent -= this.DownloadedDirectoryProgressEvent;

            package.Downloading = false;
            package.AvailableLocally = true;

            var path = Path.Combine(this._extractionFolder, package.PackageName);
            package.PrepareImages(path);

            return await Task.FromResult(package);
        }

        private void DownloadedDirectoryProgressEvent(object sender, DownloadDirectoryProgressArgs e)
        {
            var item = this._currentlyDownloadedPackages.FirstOrDefault(o => o.PackageName == ((TransferUtilityDownloadDirectoryRequest)sender).S3Directory);
            if (item == null)
            {
                return;
            }

            item.TotalBytes = e.TotalBytes;
            item.TransferredBytes = e.TransferredBytes;
            item.DownloadProgress = (e.TransferredBytes / (double)e.TotalBytes) * 100;
        }

        public async Task UploadPackageAsync(string packagePath)
        {
            var fileTransferUtility = new TransferUtility(this._client);
            var uploadRequest = new TransferUtilityUploadRequest
            {
                FilePath = packagePath,
                BucketName = this._bucketName
            };
            uploadRequest.UploadProgressEvent += this.UploadProgress;
            await fileTransferUtility.UploadAsync(uploadRequest).ConfigureAwait(false);
        }

        private void UploadProgress(object sender, UploadProgressArgs e)
        {
            //TODO: Show upload progress
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
            foreach (var image in package.Images.Where(o => o.BoundingBoxes != null))
            {
                info.Images.Add(new AnnotationImageDto
                {
                    ImageName = image.ImageName,
                    BoundingBoxes = image.BoundingBoxes
                });
            }

            try
            {
                using (var context = new DynamoDBContext(this._dynamoDbClient))
                {
                    await context.SaveAsync(info).ConfigureAwait(false);
                }
            } catch (Exception e)
            {

            }

            this._syncedPackages++;
            return true;
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
