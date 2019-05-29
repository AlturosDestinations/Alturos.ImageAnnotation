using Alturos.ImageAnnotation.Model;

namespace Alturos.ImageAnnotation.Contract
{
    public interface IAnnotationExportProvider
    {
        void Setup(AnnotationConfig config);
        void Export(string path, AnnotationPackage[] packages);
    }
}
