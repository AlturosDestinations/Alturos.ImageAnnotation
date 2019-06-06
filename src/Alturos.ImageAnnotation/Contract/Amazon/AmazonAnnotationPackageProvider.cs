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
using System.Threading;
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
        private readonly string _dbTableName;
        private readonly Queue<AnnotationPackage> _packagesToDownload;
        private readonly string _configHashKey = "AnnotationConfiguration";

        private AnnotationPackage _downloadedPackage;
        private AnnotationPackageTransferProgress _uploadProgress;
        private AnnotationPackageTransferProgress _syncProgress;

        public AmazonAnnotationPackageProvider()
        {
            var accessKeyId = ConfigurationManager.AppSettings["accessKeyId"];
            var secretAccessKey = ConfigurationManager.AppSettings["secretAccessKey"];

            this._bucketName = ConfigurationManager.AppSettings["bucketName"]?.ToLower();
            this._extractionFolder = ConfigurationManager.AppSettings["extractionFolder"];

            this._dbTableName = ConfigurationManager.AppSettings["dbTableName"];

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
                    var dbConfig = new DynamoDBOperationConfig
                    {
                        OverrideTableName = this._dbTableName
                    };
                    await context.SaveAsync(annotationConfig, dbConfig).ConfigureAwait(false);
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
                    var dbConfig = new DynamoDBOperationConfig
                    {
                        OverrideTableName = this._dbTableName
                    };
                    var annotationConfig = await context.LoadAsync<AnnotationConfigDto>(this._configHashKey, dbConfig).ConfigureAwait(false);
                    return new AnnotationConfig
                    {
                        ObjectClasses = annotationConfig.ObjectClasses,
                        Tags = annotationConfig.Tags.Select(o => new Model.AnnotationPackageTag { Value = o }).ToList()
                    };
                }
            }
            catch (Exception exception)
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
                var dbConfig = new DynamoDBOperationConfig
                {
                    OverrideTableName = this._dbTableName
                };

                try
                {
                    var allRetrievedPackages = new List<AnnotationPackageDto>();
                    var retrievedPackages = new List<AnnotationPackageDto>[scanConditions.Length];

                    for (var i = 0; i < scanConditions.Length; i++)
                    {
                        var packageInfos = context.ScanAsync<AnnotationPackageDto>(new ScanCondition[] { scanConditions[i] }, dbConfig);
                        retrievedPackages[i] = await packageInfos.GetNextSetAsync().ConfigureAwait(false);
                    }

                    allRetrievedPackages = retrievedPackages[0];
                    for (var i = 1; i < retrievedPackages.Length; i++)
                    {
                        allRetrievedPackages = allRetrievedPackages.Intersect(retrievedPackages[i]).ToList();
                    }

                    //var packages = retrievedPackages.Where(o => o.Id == "S-1-16142028098304285338.zip").Select(o => new AnnotationPackage
                    var packages = allRetrievedPackages.Select(o => new AnnotationPackage
                    {
                        ExternalId = o.Id,
                        User = o.User,
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

        public async Task<AnnotationPackage> DownloadPackageAsync(AnnotationPackage package, CancellationToken token = default(CancellationToken))
        {
            this._packagesToDownload.Enqueue(package);

            package.Enqueued = true;

            while (this._packagesToDownload.Peek() != package)
            {
                await Task.Delay(1000).ConfigureAwait(false);
            }

            package.Enqueued = false;

            this._downloadedPackage = package;

            if (!Directory.Exists(this._extractionFolder))
            {
                Directory.CreateDirectory(this._extractionFolder);
            }

            var packagePath = Path.Combine(this._extractionFolder, package.PackageName);
            if (Directory.Exists(packagePath))
            {
                Directory.Delete(packagePath, true);
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
                LocalDirectory = packagePath
            };

            request.DownloadedDirectoryProgressEvent += this.DownloadedDirectoryProgressEvent;

            try
            {
                using (var fileTransferUtility = new TransferUtility(this._client))
                {
                    await fileTransferUtility.DownloadDirectoryAsync(request, token).ConfigureAwait(false);
                }
            }
            finally
            {
                request.DownloadedDirectoryProgressEvent -= this.DownloadedDirectoryProgressEvent;
                package.Downloading = false;
            }

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

        public async Task UploadPackagesAsync(List<string> packagePaths, List<string> tags, string user, CancellationToken token = default(CancellationToken))
        {
            this.IsUploading = true;

            this._uploadProgress = new AnnotationPackageTransferProgress();

            foreach (var packagePath in packagePaths)
            {
                var fileCount = Directory.GetFiles(packagePath).Count();
                this._uploadProgress.FileCount += fileCount;
            }

            try
            {
                foreach (var packagePath in packagePaths)
                {
                    await this.UploadPackageAsync(packagePath, tags, user, token).ConfigureAwait(false);
                }
            }
            finally
            {
                this.IsUploading = false;
            }
        }

        private async Task UploadPackageAsync(string packagePath, List<string> tags, string user, CancellationToken token)
        {
            var packageName = Path.GetFileName(packagePath);
            await this.AddPackageAsync(new AnnotationPackage
            {
                ExternalId = packageName,
                User = user,
                PackageName = packageName,
                IsAnnotated = false,
                AnnotationPercentage = 0,
                Images = new List<AnnotationImage>(),
                Tags = tags
            }, token).ConfigureAwait(false);

            var files = Directory.GetFiles(packagePath);
            foreach (var file in files)
            {
                await this.UploadFileAsync(file, token).ConfigureAwait(false);
            }
        }

        private async Task UploadFileAsync(string filePath, CancellationToken token)
        {
            using (var fileTransferUtility = new TransferUtility(this._client))
            {
                var keyName = $"{Directory.GetParent(filePath).Name}/{Path.GetFileName(filePath)}";

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    Key = keyName,
                    FilePath = filePath,
                    BucketName = this._bucketName
                };

                var uploadProgressEventHandler = new EventHandler<UploadProgressArgs>((sender, uploadProgressArgs) =>
                {
                    this._uploadProgress.CurrentFile = uploadProgressArgs.FilePath;
                    this._uploadProgress.CurrentFilePercentDone = uploadProgressArgs.PercentDone;
                });

                uploadRequest.UploadProgressEvent += uploadProgressEventHandler;

                await fileTransferUtility.UploadAsync(uploadRequest, token)
                    .ContinueWith(o => uploadRequest.UploadProgressEvent -= uploadProgressEventHandler)
                    .ConfigureAwait(false);
            }

            this._uploadProgress.UploadedFiles++;
            this._uploadProgress.CurrentFilePercentDone = 0;
        }

        public async Task SyncPackagesAsync(AnnotationPackage[] packages, CancellationToken token = default(CancellationToken))
        {
            this.IsSyncing = true;

            this._syncProgress = new AnnotationPackageTransferProgress()
            {
                FileCount = packages.Length
            };

            try
            {
                foreach (var package in packages)
                {
                    await this.UpdatePackageAsync(package, token).ConfigureAwait(false);
                }
            }
            finally
            {
                this.IsSyncing = false;
            }
        }

        private async Task<bool> UpdatePackageAsync(AnnotationPackage package, CancellationToken token)
        {
            using (var context = new DynamoDBContext(this._dynamoDbClient))
            {
                var dbConfig = new DynamoDBOperationConfig
                {
                    OverrideTableName = this._dbTableName
                };
                var info = await context.LoadAsync<AnnotationPackageDto>(package.ExternalId, dbConfig, token).ConfigureAwait(false);

                info.User = package.User;
                info.IsAnnotated = package.IsAnnotated;
                info.AnnotationPercentage = package.AnnotationPercentage;
                info.Tags = package.Tags;

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

                await context.SaveAsync(info, dbConfig, token).ConfigureAwait(false);

                this._syncProgress.UploadedFiles++;
                return true;
            }
        }

        private async Task<bool> AddPackageAsync(AnnotationPackage package, CancellationToken token)
        {
            var info = new AnnotationPackageDto
            {
                Id = package.ExternalId,
                User = package.User,
                IsAnnotated = package.IsAnnotated,
                AnnotationPercentage = package.AnnotationPercentage,
                Tags = package.Tags,
                Images = new List<AnnotationImageDto>()
            };

            using (var context = new DynamoDBContext(this._dynamoDbClient))
            {
                var dbConfig = new DynamoDBOperationConfig
                {
                    OverrideTableName = this._dbTableName
                };
                await context.SaveAsync(info, dbConfig, token).ConfigureAwait(false);
            }

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
                    var dbConfig = new DynamoDBOperationConfig
                    {
                        OverrideTableName = this._dbTableName
                    };
                    await context.DeleteAsync(new AnnotationPackageDto
                    {
                        Id = package.ExternalId
                    }, dbConfig).ConfigureAwait(false);
                }

                // Delete local folder
                var path = Path.Combine(this._extractionFolder, package.PackageName);
                if (Directory.Exists(path)) {
                    Directory.Delete(path, true);
                }

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
                    var dbConfig = new DynamoDBOperationConfig
                    {
                        OverrideTableName = this._dbTableName
                    };
                    var package = await context.LoadAsync<AnnotationPackageDto>(image.Package.ExternalId, dbConfig).ConfigureAwait(false);
                    package.Images?.RemoveAll(o => o.ImageName.Equals(image.ImageName));
                    await context.SaveAsync(package, dbConfig).ConfigureAwait(false);
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

        public AnnotationPackageTransferProgress GetUploadProgress()
        {
            if (!this.IsUploading)
            {
                return null;
            }

            return this._uploadProgress;
        }

        public AnnotationPackageTransferProgress GetSyncProgress()
        {
            if (!this.IsSyncing)
            {
                return null;
            }

            return this._syncProgress;
        }
    }
}
