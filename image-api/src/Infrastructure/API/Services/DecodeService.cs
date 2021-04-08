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
        public byte[] DecodeMessage(Bitmap image)
        {
            int length = image.DecodeMessageLength();
            var query = DecodeMessageImpl(image, length * 8);

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

        private static IEnumerable<byte> DecodeMessageImpl(Bitmap image, int messageLength)
        {
            ushort seed = image.DecodeSeed();
            var pixelEnumerator = image.RandomDistribution(seed).GetEnumerator();

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
                    pixelEnumerator = image.RandomDistribution(seed).GetEnumerator();
                    mask <<= 1;
                    shiftRight++;
                }
            }

            pixelEnumerator.Dispose();
        }
    }
}
