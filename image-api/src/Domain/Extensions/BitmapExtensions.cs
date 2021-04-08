using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Domain.Extensions
{
    public static class BitmapExtensions
    {
        public static Pixel GetPixelValue(this Bitmap image, int x, int y)
        {
            var pixel = image.GetPixel(x, y);

            return new Pixel(pixel.R, pixel.G, pixel.B);
        }

        public static IEnumerable<(Point Point, Pixel Pixel)> RandomDistribution(this Bitmap image)
        {
            int numPixel = image.Width * image.Height;
            var permutaion = new Random(420).Permutation(2, numPixel - 3);

            return permutaion.Select(n =>
            {
                int y = Math.DivRem(n, image.Width, out int x);
                Point point = new(x, y);
                Pixel pixel = image.GetPixelValue(x, y);

                return (point, pixel);
            });
        }
    }
}

