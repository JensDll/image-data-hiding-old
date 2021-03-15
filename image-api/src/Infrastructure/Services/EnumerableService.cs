using Application.Common.Interfaces.Services;
using Domain.Common.DataStructures;
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
            foreach (byte b in bytes)
            {
                yield return (byte)((b & 128) >> 7);
                yield return (byte)((b & 64) >> 6);
                yield return (byte)((b & 32) >> 5);
                yield return (byte)((b & 16) >> 4);
                yield return (byte)((b & 8) >> 3);
                yield return (byte)((b & 4) >> 2);
                yield return (byte)((b & 2) >> 1);
                yield return (byte)(b & 1);
            }
        }

        public IEnumerable<(Point, Pixel)> EvenDistribution(Bitmap image)
        {
            Heap<Range> maxHeap = new((r1, r2) => r2.GetLength() - r1.GetLength());

            int end = (image.Width * image.Height) - 1;
            maxHeap.Add(0..end);

            while (maxHeap.Count > 0)
            {
                Range range = maxHeap.RemoveTop();

                if (range.TryBisect(out (Range Left, Range Right) ranges))
                {
                    maxHeap.Add(ranges.Left).Add(ranges.Right);
                }
                else
                {
                    int y = Math.DivRem(range.Start.Value, image.Width, out int x);
                    Point point = new (x, y);
                    Pixel pixel = image.GetPixelValue(x, y);

                    yield return (point, pixel);
                }
            }
        }
    }
}
