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

        // ================ Pixel Distribution ================
        public static IEnumerable<(Point Point, Pixel Pixel)> RandomDistribution(this Bitmap image, int seed)
        {
            int numPixel = image.Width * image.Height;
            var permutaion = new Random(seed).Permutation(2, numPixel - 4);

            return permutaion.Select(n =>
            {
                int y = Math.DivRem(n, image.Width, out int x);
                Point point = new(x, y);
                Pixel pixel = image.GetPixelValue(x, y);

                return (point, pixel);
            });
        }

        // ================ Message length ================
        public static void EncodeMessageLength(this Bitmap image, int length)
        {
            var (r1, g1, b1) = image.GetPixelValue(0, 0); // 12 bit (rgb)
            var (r2, g2, b2) = image.GetPixelValue(1, 0);  // 4 bit (r)
            var (r3, g3, b3) = image.GetPixelValue(image.Width - 2, image.Height - 1); // 4 bit (r)
            var (r4, g4, b4) = image.GetPixelValue(image.Width - 1, image.Height - 1); // 12 bit (rgb)
            
            byte low = 0b_0000_1111;
            byte high = 0b_1111_0000;

            // 12 bit
            r1 = r1 & high | length & low;
            g1 = g1 & high | (length >> 4) & low;
            b1 = b1 & high | (length >> 8) & low;

            // 4 bit
            r2 = r2 & high | (length >> 12) & low;

            // 4 bit
            r3 = r3 & high | (length >> 16) & low;

            // 12 bit
            r4 = r4 & high | (length >> 20) & low;
            g4 = g4 & high | (length >> 24) & low;
            b4 = b4 & high | (length >> 28) & low;

            image.SetPixel(0, 0, Color.FromArgb(r1, g1, b1));
            image.SetPixel(1, 0, Color.FromArgb(r2, g2, b2));
            image.SetPixel(image.Width - 2, image.Height - 1, Color.FromArgb(r3, g3, b3));
            image.SetPixel(image.Width - 1, image.Height - 1, Color.FromArgb(r4, g4, b4));
        }

        public static int DecodeMessageLength(this Bitmap image)
        {
            var (r1, g1, b1) = image.GetPixelValue(0, 0); // 12 bit (rgb)
            var (r2, _, _) = image.GetPixelValue(1, 0);  // 4 bit (r)
            var (r3, _, _) = image.GetPixelValue(image.Width - 2, image.Height - 1); // 4 bit (r)
            var (r4, g4, b4) = image.GetPixelValue(image.Width - 1, image.Height - 1); // 12 bit (rgb)

            byte low = 0b_0000_1111;

            // 12 bit
            int a = r1 & low;
            int b = (g1 & low) << 4;
            int c = (b1 & low) << 8;

            // 4 bit
            int d = (r2 & low) << 12;

            // 4 bit
            int e = (r3 & low) << 16;

            // 12 bit
            int f = (r4 & low) << 20;
            int g = (g4 & low) << 24;
            int h = (b4 & low) << 28;

            return a | b | c | d | e | f | g | h;
        }

        // ================ Seed ================
        public static void EncodeSeed(this Bitmap image, ushort seed)
        {
            var (r1, g1, b1) = image.GetPixelValue(1, 0);  // 8 bit (gb)
            var (r2, g2, b2) = image.GetPixelValue(image.Width - 2, image.Height - 1); // 8 bit (gb)

            byte low = 0b_0000_1111;
            byte high = 0b_1111_0000;

            g1 = g1 & high | seed & low;
            b1 = b1 & high | (seed >> 4) & low;

            g2 = g2 & high | (seed >> 8) & low;
            b2 = b2 & high | (seed >> 12) & low;

            image.SetPixel(1, 0, Color.FromArgb(r1, g1, b1));
            image.SetPixel(image.Width - 2, image.Height - 1, Color.FromArgb(r2, g2, b2));
        }

        public static ushort DecodeSeed(this Bitmap image)
        {
            var (_, g1, b1) = image.GetPixelValue(1, 0);  // 8 bit (gb)
            var (_, g2, b2) = image.GetPixelValue(image.Width - 2, image.Height - 1); // 8 bit (gb)

            byte low = 0b_0000_1111;

            // 8 bit
            int a = g1 & low;
            int b = (b1 & low) << 4;

            // 8 bit
            int c = (g2 & low) << 8;
            int d = (b2 & low) << 12;

            return (ushort)(a | b | c | d);
        }
    }
}

