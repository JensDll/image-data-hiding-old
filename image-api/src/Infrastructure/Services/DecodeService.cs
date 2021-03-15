using Application.Common.Interfaces.Services;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DecodeService : IDecodeService
    {
        private IEnumerableService EnumerableService { get; }

        public DecodeService(IEnumerableService enumerableService)
        {
            EnumerableService = enumerableService;
        }

        public byte[] DecodeMessage(Bitmap image)
        {
            int length = DecodeMessageLength(image);
            var pixelSequence = EnumerableService.EvenDistribution(image);
            var query = DecodeMessageImpl(image, pixelSequence, BitPosition.One, length * 8);

            var result = new byte[length];

            int arrayIndex = 0;
            int i = 0;
            byte b = 0;
            foreach (byte bit in query)
            {
                b |= bit;

                if (++i == 8)
                {
                    result[arrayIndex++] = b;
                    i = b = 0;
                }
                else
                {
                    b <<= 1;
                }
            }

            return result;
        }

        private IEnumerable<byte> DecodeMessageImpl(Bitmap image,
            IEnumerable<(Point, Pixel)> pixelSequence,
            BitPosition bitPosition,
            int messageLength)
        {
            byte mask = (byte)bitPosition;

            foreach (var (point, (R, G, B)) in pixelSequence)
            {
                if (ShouldSkip(point, image)) continue;

                if (messageLength-- > 0)
                    yield return (byte)(R & mask);
                if (messageLength-- > 0)
                    yield return (byte)(G & mask);
                if (messageLength-- > 0)
                    yield return (byte)(B & mask);
            }

            if (messageLength > 0)
            {
                DecodeMessageImpl(image, pixelSequence, (BitPosition)((int)bitPosition << 1), messageLength);
            }
        }

        private int DecodeMessageLength(Bitmap image)
        {
            var (r1, g1, b1) = image.GetPixelValue(0, 0);
            var (r2, g2, b2) = image.GetPixelValue(image.Width - 1, image.Height - 1);

            byte mask = 0b_0000_1111;

            int a = r1 & mask;
            int b = (g1 & mask) << 4;
            int c = (b1 & mask) << 8;
            int d = (r2 & mask) << 12;
            int e = (g2 & mask) << 16;
            int f = (b2 & mask) << 20;

            return a + b + c + d + e + f;
        }

        private static bool ShouldSkip(Point point, Bitmap image) =>
            point == new Point(0, 0) || point == new Point(image.Width - 1, image.Height - 1);
    }
}
