using Application.API.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.API.Services
{
    internal class EncodeService : IEncodeService
    {
        private readonly Dictionary<BitPosition, byte> lookup = new()
        {
            { BitPosition.One, 0 },
            { BitPosition.Two, 1 },
            { BitPosition.Three, 2 },
            { BitPosition.Four, 3 },
            { BitPosition.Five, 4 },
            { BitPosition.Six, 5 },
            { BitPosition.Seven, 6 },
            { BitPosition.Eighth, 7 },
        };

        public void EnocodeMessage(Bitmap image, byte[] message)
        {
            EncodeMessageLength(image, message.Length);
            EncodeMessageImpl(image, message.Bitwise().GetEnumerator());
        }

        private void EncodeMessageImpl(Bitmap image,
            IEnumerator<byte> bitSequence,
            BitPosition bitPosition = BitPosition.One)
        {
            byte shift = lookup[bitPosition];
            byte mask = (byte)~bitPosition;

            foreach (var (point, pixel) in image.RandomDistribution())
            {
                int r = pixel.R;
                int g = pixel.G;
                int b = pixel.B;

                if (bitSequence.MoveNext())
                    r = r & mask | (bitSequence.Current << shift);
                else
                {
                    bitSequence.Dispose();
                    image.SetPixel(point.X, point.Y, Color.FromArgb(r, g, b));
                    return;
                }

                if (bitSequence.MoveNext())
                    g = g & mask | (bitSequence.Current << shift);
                else
                {
                    bitSequence.Dispose();
                    image.SetPixel(point.X, point.Y, Color.FromArgb(r, g, b));
                    return;
                }

                if (bitSequence.MoveNext())
                    b = b & mask | (bitSequence.Current << shift);
                else
                {
                    bitSequence.Dispose();
                    image.SetPixel(point.X, point.Y, Color.FromArgb(r, g, b));
                    return;
                }

                image.SetPixel(point.X, point.Y, Color.FromArgb(r, g, b));
            }

            if (bitPosition == BitPosition.Eighth)
            {
                throw new MessageToLongException();
            }

            EncodeMessageImpl(image, bitSequence, (BitPosition)((byte)bitPosition << 1));
        }

        private static void EncodeMessageLength(Bitmap image, int length)
        {
            var (r1, g1, b1) = image.GetPixelValue(0, 0); // 1.5 byte
            var (r2, g2, b2) = image.GetPixelValue(1, 0);  // 1 byte
            var (r3, g3, b3) = image.GetPixelValue(image.Width - 1, image.Height - 1); // 1.5 byte

            byte mask1 = 0b_0000_1111;
            byte mask2 = 0b_1111_0000;

            r1 = r1 & mask2 | length & mask1;
            g1 = g1 & mask2 | (length >> 4) & mask1;
            b1 = b1 & mask2 | (length >> 8) & mask1;

            r2 = r2 & mask2 | (length >> 12) & mask1;
            g2 = g2 & mask2 | (length >> 16) & mask1;

            r3 = r3 & mask2 | (length >> 20) & mask1;
            g3 = g3 & mask2 | (length >> 24) & mask1;
            b3 = b3 & mask2 | (length >> 28) & mask1;

            image.SetPixel(0, 0, Color.FromArgb(r1, g1, b1));
            image.SetPixel(1, 0, Color.FromArgb(r2, g2, b2));
            image.SetPixel(image.Width - 1, image.Height - 1, Color.FromArgb(r3, g3, b3));
        }
    }
}
