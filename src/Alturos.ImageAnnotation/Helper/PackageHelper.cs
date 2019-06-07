using System.IO;
using System.Linq;

namespace Alturos.ImageAnnotation.Helper
{
    public static class PackageHelper
    {
        public static readonly string[] AllowedExtensions = new[] { ".png", ".jpg", ".bmp" };

        public static string[] GetImages(string directory)
        {
            return Directory.GetFiles(directory).Where(file => AllowedExtensions.Any(file.ToLower().EndsWith)).ToArray();
        }
    }
}
