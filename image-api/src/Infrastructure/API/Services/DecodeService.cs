using Application.API.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.API.Services
{
    internal class DecodeService : IDecodeService
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
            var query = DecodeMessageImpl(image, pixelSequence, length * 8);

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

        private static IEnumerable<byte> DecodeMessageImpl(Bitmap image,
            IEnumerable<(Point, Pixel)> pixelSequence,
            int messageLength)
        {
            var pixelEnumerator = pixelSequence.GetEnumerator();

            byte mask = (byte)BitPosition.One;
            int shiftRight = 0;

            while (messageLength > 0)
            {
                if (pixelEnumerator.MoveNext())
                {
                    var (_, pixel) = pixelEnumerator.Current;

                    if (messageLength-- > 0)
                        yield return (byte)((pixel.R & mask) >> shiftRight);
                    if (messageLength-- > 0)
                        yield return (byte)((pixel.G & mask) >> shiftRight);
                    if (messageLength-- > 0)
                        yield return (byte)((pixel.B & mask) >> shiftRight);
                }
                else
                {
                    pixelEnumerator = pixelSequence.GetEnumerator();
                    mask <<= 1;
                    shiftRight++;
                }
            }

            pixelEnumerator.Dispose();
        }

        private static int DecodeMessageLength(Bitmap image)
        {
            var (r1, g1, b1) = image.GetPixelValue(0, 0); // 1.5 byte
            var (r2, g2, _) = image.GetPixelValue(1, 0); // 1 byte 
            var (r3, g3, b3) = image.GetPixelValue(image.Width - 1, image.Height - 1); // 1.5 byte

            byte mask = 0b_0000_1111;

            int a = r1 & mask;
            int b = (g1 & mask) << 4;
            int c = (b1 & mask) << 8;

            int d = (r2 & mask) << 12;
            int e = (g2 & mask) << 16;

            int f = (r3 & mask) << 20;
            int g = (g3 & mask) << 24;
            int h = (b3 & mask) << 28;

            return a | b | c | d | e | f | g | h;
        }
    }
}
