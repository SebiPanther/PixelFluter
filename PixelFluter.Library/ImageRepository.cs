namespace PixelFluter.Library
{
    using System;
    using ImageSharp = SixLabors.ImageSharp.Image;
    using ImageExtension = SixLabors.ImageSharp.ImageExtensions;
    using System.Collections.Generic;
    using System.Drawing;
    internal class ImageRepository
    {
        internal List<Pixel> ReadImage(string path)
        {
            var result = new List<Pixel>();
            byte[] imageBytes = null;
            using(var image = ImageSharp.Load(path))
            {
                imageBytes = ImageExtension.SavePixelData(image);
                for(int x=0;x<image.Width;x++)
                {
                    for(int y=0;y<image.Height;y++)
                    {
                        var baseIndex = (x + (y * image.Width)) * 4;
                        if(imageBytes[baseIndex +3] > 0)
                        {
                            result.Add(new Pixel()
                            {
                                X = x,
                                Y = y,
                                R = imageBytes[baseIndex],
                                G = imageBytes[baseIndex +1],
                                B = imageBytes[baseIndex +2],
                                A = imageBytes[baseIndex +3]
                            });
                        }
                    }
                }
            }
            return result;
        }
    }
}
