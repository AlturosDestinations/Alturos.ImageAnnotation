using Alturos.ImageAnnotation.Model;
using System;
using System.Drawing;

namespace Alturos.ImageAnnotation.Helper
{
    public static class DrawHelper
    {
        public static readonly Size ImageSize = new Size(1024, 576);

        private static readonly string[] Colors = new string[] { "#E3330E", "#48E10F", "#D40FE1", "#24ECE3", "#EC2470" };

        public static Image DrawBoxes(AnnotationImage image)
        {
            try
            {
                var originalBitmap = new Bitmap(image.ImagePath);

                var newImageSize = new Size();
                if (originalBitmap.Width > ImageSize.Width)
                {
                    newImageSize.Height = (int)(originalBitmap.Height * (ImageSize.Width / (double)originalBitmap.Width));
                    newImageSize.Width = ImageSize.Width;
                }
                if (originalBitmap.Height > ImageSize.Height)
                {
                    newImageSize.Width = (int)(originalBitmap.Width * (ImageSize.Height / (double)originalBitmap.Height));
                    newImageSize.Height = ImageSize.Height;
                }

                var resizedBitmap = new Bitmap(originalBitmap, newImageSize);
                foreach (var id in originalBitmap.PropertyIdList)
                {
                    resizedBitmap.SetPropertyItem(originalBitmap.GetPropertyItem(id));
                }
  
                originalBitmap.Dispose();

                return resizedBitmap;
            }
            catch (Exception)
            {
                var bitmap = new Bitmap(ImageSize.Width, ImageSize.Height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.White);
                }

                return bitmap;
            }
        }

        public static Color GetColorCode(int index)
        {
            return ColorTranslator.FromHtml(Colors[index]);
        }
    }
}
