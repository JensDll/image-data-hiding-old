using Application.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        public bool WriteMessage(Bitmap image, byte[] message)
        {
            WriteMessageLength(image, message.Length);

            var messageIter = MessageIterator(message).GetEnumerator();

            foreach (var ((x, y), pixel) in ImageIterator(image))
            {
                if (IsFirstPixel(x, y) || IsLastPixel(image, x, y))
                    continue;

                int r = pixel.R;
                int g = pixel.G;
                int b = pixel.B;

                for (int i = 0; i < 3; i++)
                {
                    if (messageIter.MoveNext())
                    {
                        switch (i)
                        {
                            case 0:
                                r = r & 252 | messageIter.Current;
                                break;
                            case 1:
                                g = g & 252 | messageIter.Current;
                                break;
                            case 2:
                                b = b & 252 | messageIter.Current;
                                break;
                        }
                    }
                    else
                    {
                        messageIter.Dispose();
                        image.SetPixel(x, y, Color.FromArgb(r, g, b));
                        return true;
                    }


                    image.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return false;
        }

        public byte[] ReadMessage(Bitmap image)
        {
            int length = ReadMessageLength(image);

            return null;
        }

        private static void WriteMessageLength(Bitmap image, int length)
        {
            var (r1, g1, b1) = image.GetPixelValues(0, 0);
            var (r2, g2, b2) = image.GetPixelValues(image.Width - 1, image.Height - 1);

            // 15  = 0b_0000_1111
            // 240 = 0b_1111_0000
            r1 = r1 & 240 | length & 15;
            g1 = g1 & 240 | (length >> 4) & 15;
            b1 = b1 & 240 | (length >> 8) & 15;

            r2 = r2 & 240 | (length >> 12) & 15;
            g2 = g2 & 240 | (length >> 16) & 15;
            b2 = b2 & 240 | (length >> 20) & 15;

            image.SetPixel(0, 0, Color.FromArgb(r1, g1, b1));
            image.SetPixel(image.Width - 1, image.Height - 1, Color.FromArgb(r2, g2, b2));
        }

        private static int ReadMessageLength(Bitmap image)
        {
            var (r1, g1, b1) = image.GetPixelValues(0, 0);
            var (r2, g2, b2) = image.GetPixelValues(image.Width - 1, image.Height - 1);

            int a = r1 & 15;
            int b = (g1 & 15) << 4;
            int c = (b1 & 15) << 8;
            int d = (r2 & 15) << 12;
            int e = (g2 & 15) << 16;
            int f = (b2 & 15) << 20;

            return a + b + c + d + e + f;
        }

        private static IEnumerable<int> MessageIterator(byte[] message)
        {
            foreach (byte b in message)
            {
                for (int i = 0; i < 8; i += 2)
                {
                    yield return (b >> i) & 3;
                }
            }
        }

        private static IEnumerable<(Point, Pixel)> ImageIterator(Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var point = new Point(x, y);
                    var pixel = image.GetPixelValues(x, y);

                    yield return (point, pixel);
                }
            }
        }

        private static bool IsFirstPixel(int x, int y) => x == 0 && y == 0;

        private static bool IsLastPixel(Bitmap image, int x, int y)
            => x == image.Width - 1 && y == image.Height - 1;
    }

    public record Point(int X, int Y);

    public record Pixel(int R, int G, int B);

    public static class BitmapExtensions
    {
        public static Pixel GetPixelValues(this Bitmap image, int x, int y)
        {
            var pixel = image.GetPixel(x, y);

            return new Pixel(pixel.R, pixel.G, pixel.B);
        }
    }
}
