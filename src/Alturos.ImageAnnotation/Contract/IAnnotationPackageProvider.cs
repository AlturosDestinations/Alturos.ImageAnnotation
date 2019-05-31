using Alturos.ImageAnnotation.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alturos.ImageAnnotation.Contract
{
    public interface IAnnotationPackageProvider
    {
        bool IsSyncing { get; }
        bool IsUploading { get; }

        Task SetAnnotationConfigAsync(AnnotationConfig config);
        Task<AnnotationConfig> GetAnnotationConfigAsync();

        Task<AnnotationPackage[]> GetPackagesAsync(bool annotated);
        Task<AnnotationPackage[]> GetPackagesAsync(AnnotationPackageTag[] tags);

        Task<AnnotationPackage> DownloadPackageAsync(AnnotationPackage package);
        Task UploadPackagesAsync(List<string> packagePath, List<string> tags);
        Task SyncPackagesAsync(AnnotationPackage[] packages);

        Task<bool> DeletePackageAsync(AnnotationPackage package);
        Task<bool> DeleteImageAsync(AnnotationImage image);

        double GetUploadProgress();
        double GetSyncProgress();
    }
}
