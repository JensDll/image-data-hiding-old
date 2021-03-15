using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Domain.Extensions
{
    public static class BitmapExtension
    {
        public static Pixel GetPixelValue(this Bitmap image, int x, int y)
        {
            var pixel = image.GetPixel(x, y);

            return new Pixel(pixel.R, pixel.G, pixel.B);
        }
    }
}

