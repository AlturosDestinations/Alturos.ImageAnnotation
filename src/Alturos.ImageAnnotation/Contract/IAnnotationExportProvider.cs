using Alturos.ImageAnnotation.Model;

namespace Alturos.ImageAnnotation.Contract
{
    public interface IAnnotationExportProvider
    {
        void Export(string path, AnnotationPackage[] packages);
    }
}
