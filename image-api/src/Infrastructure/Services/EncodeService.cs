using Application.Common.Interfaces;
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

namespace Infrastructure.Services
{
    public class EncodeService : IEncodeService
    {
        private readonly IEnumerableService enumerableService;

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

        public EncodeService(IEnumerableService enumerableService)
        {
            this.enumerableService = enumerableService;
        }

        public void EnocodeMessage(Bitmap image, byte[] message)
        {
            EncodeMessageLength(image, message.Length);

            var pixelSequence = enumerableService.EvenDistribution(image);
            var bitSequence = enumerableService.Bitwise(message).GetEnumerator();

            EncodeMessageImpl(image, pixelSequence, bitSequence);
        }

        private void EncodeMessageImpl(Bitmap image,
            IEnumerable<(Point, Pixel)> pixelSequence,
            IEnumerator<byte> bitSequence,
            BitPosition bitPosition = BitPosition.One)
        {
            byte shift = lookup[bitPosition];
            byte mask = (byte)~bitPosition;

            foreach (var (point, pixel) in pixelSequence)
            {
                if (ShouldSkip(point, image)) continue;

                int r = pixel.R;
                int g = pixel.G;
                int b = pixel.B;

                for (int i = 0; i < 3; i++)
                {
                    if (bitSequence.MoveNext())
                    {
                        int current = bitSequence.Current << shift;

                        switch (i)
                        {
                            case 0:
                                r = r & mask | current;
                                break;
                            case 1:
                                g = g & mask | current;
                                break;
                            case 2:
                                b = b & mask | current;
                                break;
                        }
                    }
                    else
                    {
                        bitSequence.Dispose();
                        image.SetPixel(point.X, point.Y, Color.FromArgb(r, g, b));
                        return;
                    }
                }

                image.SetPixel(point.X, point.Y, Color.FromArgb(r, g, b));
            }

            if (bitPosition == BitPosition.Eighth)
            {
                throw new MessageToLongException();
            }

            EncodeMessageImpl(image, pixelSequence, bitSequence, (BitPosition)((byte)bitPosition << 1));
        }

        private static bool ShouldSkip(Point point, Bitmap image) =>
            point == new Point(0, 0) || point == new Point(image.Width - 1, image.Height - 1);

        private static void EncodeMessageLength(Bitmap image, int length)
        {
            var (r1, g1, b1) = image.GetPixelValue(0, 0);
            var (r2, g2, b2) = image.GetPixelValue(image.Width - 1, image.Height - 1);

            byte mask1 = 0b_0000_1111;
            byte mask2 = 0b_1111_0000;

            r1 = r1 & mask2 | length & mask1;
            g1 = g1 & mask2 | (length >> 4) & mask1;
            b1 = b1 & mask2 | (length >> 8) & mask1;

            r2 = r2 & mask2 | (length >> 12) & mask1;
            g2 = g2 & mask2 | (length >> 16) & mask1;
            b2 = b2 & mask2 | (length >> 20) & mask1;

            image.SetPixel(0, 0, Color.FromArgb(r1, g1, b1));
            image.SetPixel(image.Width - 1, image.Height - 1, Color.FromArgb(r2, g2, b2));
        }
    }
}
