using Alturos.ImageAnnotation.Model;
using System.Collections.Generic;

namespace Alturos.ImageAnnotation.Contract
{
    public interface IAnnotationExportProvider
    {
        void Setup(AnnotationConfig config);
        void Export(string path, AnnotationPackage[] packages, ObjectClass[] objectClasses);
    }
}
