using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Infrastructure.Services
{
    public class EnumerableService : IEnumerableService
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
            Queue<RangeRecord> queue = new();
            int end = (image.Width * image.Height) - 1;
            queue.Enqueue(new RangeRecord(0, end));

            while (queue.Count > 0)
            {
                RangeRecord range = queue.Dequeue();

                if (range.TryBisect(out var ranges))
                {
                    queue.Enqueue(ranges.Left);
                    queue.Enqueue(ranges.Right);
                }
                else
                {
                    int y = Math.DivRem(range.Start, image.Width, out int x);
                    Point point = new(x, y);
                    Pixel pixel = image.GetPixelValue(x, y);

                    yield return (point, pixel);
                }
            }
        }
    }
}
