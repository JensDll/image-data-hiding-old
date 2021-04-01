using Application.API.Interfaces;
using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infrastructure.API.Services
{
    internal class EnumerableService : IEnumerableService
    {
        public IEnumerable<byte> Bitwise(byte[] bytes)
        {
            var masks = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };

            foreach (byte b in bytes)
            {
                for (int i = 7; i >= 0; i--)
                {
                    yield return (byte)((b & masks[i]) >> i);
                }
            }
        }

        public IEnumerable<(Point Point, Pixel Pixel)> EvenDistribution(Bitmap image)
        {
            (Point Point, Pixel Pixel) GetValues(int x, int y)
            {
                Point point = new(x, y);
                Pixel pixel = image.GetPixelValue(x, y);

                return (point, pixel);
            };

            Queue<RangeRecord> queue = new();
            int end = (image.Width * image.Height) - 1;
            queue.Enqueue(new RangeRecord(2, end - 1));

            while (queue.Count > 0)
            {
                RangeRecord range = queue.Dequeue();

                if (range.TryBisect(out var ranges))
                {
                    queue.Enqueue(ranges.Left);
                    queue.Enqueue(ranges.Right);

                    if (ranges.middle != -1)
                    {
                        int y = Math.DivRem(ranges.middle, image.Width, out int x);
                        yield return GetValues(x, y);
                    }
                }
                else
                {
                    int y = Math.DivRem(range.Start, image.Width, out int x);
                    yield return GetValues(x, y);
                }
            }
        }
    }
}
