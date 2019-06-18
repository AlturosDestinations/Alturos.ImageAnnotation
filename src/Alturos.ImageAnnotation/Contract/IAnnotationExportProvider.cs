using Alturos.ImageAnnotation.Model;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Alturos.ImageAnnotation.Contract
{
    public interface IAnnotationExportProvider
    {
        Control Control { get; set; }

        void Setup(AnnotationConfig config);
        void Export(string path, AnnotationPackage[] packages, ObjectClass[] objectClasses);
    }
}
