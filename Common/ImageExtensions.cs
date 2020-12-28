using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading.Tasks;

namespace GrowthDiary.Common
{
    public static class ImageExtensions
    {
        public static Image Crop(this Image b, Rectangle r)
        {
            var nb = new Bitmap(r.Width, r.Height);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, -r.X, -r.Y);
                return nb;
            }
        }

        public static Image Crop(this Image b, int x, int y, int width, int height)
        {
            return b.Crop(new Rectangle(x, y, width, height));
        }

        public static Image Resize(this Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            //destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public static Image Shrink(this Image image, int size)
        {
            if (image.Width <= size && image.Height <= size)
            {
                return image;
            }
            if (image.Width > image.Height)
            {
                var height = image.Height * size / image.Width;
                return image.Resize(size, height);
            }
            var width = image.Width * size / image.Height;
            return image.Resize(width, size);
        }


        public static string GetFileExtension(this ImageFormat imageFormat)
        {
            var extension = ImageCodecInfo.GetImageEncoders()
                .Where(ie => ie.FormatID == imageFormat.Guid)
                .Select(ie => ie.FilenameExtension
                    .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .First()
                    .Trim('*')
                    .ToLower())
                .FirstOrDefault();

            return extension ?? string.Format(".{0}", imageFormat.ToString().ToLower());
        }
    }
}
