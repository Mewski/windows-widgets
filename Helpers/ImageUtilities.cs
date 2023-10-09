using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Media;

namespace WindowsWidgets
{
    public static class ImageUtilities
    {
        public static Bitmap PixelateImage(Bitmap image, Rectangle rectangle, int pixelateSize)
        {
            Bitmap pixelated = new Bitmap(image.Width, image.Height);

            using (Graphics graphics = Graphics.FromImage(pixelated))
            {
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
            }

            for (int currentX = rectangle.X; currentX < rectangle.X + rectangle.Width && currentX < image.Width; currentX += pixelateSize)
            {
                for (int currentY = rectangle.Y; currentY < rectangle.Y + rectangle.Height && currentY < image.Height; currentY += pixelateSize)
                {
                    int offsetX = pixelateSize / 2;
                    int offsetY = pixelateSize / 2;

                    while (currentX + offsetX >= image.Width)
                    { 
                        offsetX--;
                    }

                    while (currentY + offsetY >= image.Height) 
                    {
                        offsetY--;
                    }

                    var pixel = pixelated.GetPixel(currentX + offsetX, currentY + offsetY);

                    for (int x = currentX; x < currentX + pixelateSize && x < image.Width; x++)
                    {
                        for (int y = currentY; y < currentY + pixelateSize && y < image.Height; y++)
                        {
                            pixelated.SetPixel(x, y, pixel);
                        }
                    }
                }
            }

            int newWidth = (int)Math.Ceiling((double)rectangle.Width / pixelateSize);
            int newHeight = (int)Math.Ceiling((double)rectangle.Height / pixelateSize);

            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            for (int currentX = 0; currentX < newWidth; currentX++)
            {
                for (int currentY = 0; currentY < newHeight; currentY++)
                {
                    int originalX = rectangle.X + currentX * pixelateSize;
                    int originalY = rectangle.Y + currentY * pixelateSize;

                    if (originalX < image.Width && originalY < image.Height)
                    {
                        var color = pixelated.GetPixel(originalX, originalY);
                        resizedImage.SetPixel(currentX, currentY, color);
                    }
                }
            }

            return resizedImage;
        }

        public static System.Windows.Media.Color[] GetColorPalette(Bitmap image)
        {
            var palette = new List<System.Windows.Media.Color>();

            for (int currentX = 0; currentX < image.Width; currentX++)
            {
                for (int currentY = 0; currentY < image.Height; currentY ++)
                {
                    var color = image.GetPixel(currentX, currentY);

                    if (palette.Contains(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B)))
                    {
                        continue;
                    }

                    palette.Add(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                }
            }

            return palette.ToArray();
        }
    }
}
