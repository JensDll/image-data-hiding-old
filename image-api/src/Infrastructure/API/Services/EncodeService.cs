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
        private readonly Random rand = new ();

        public void EnocodeMessage(Bitmap image, byte[] message)
        {
            ushort seed = (ushort)rand.Next();
            image.EncodeMessageLength(message.Length);
            image.EncodeSeed(seed);
            EncodeMessageImpl(image, message.Bitwise().GetEnumerator(), seed);
        }

        private void EncodeMessageImpl(Bitmap image, IEnumerator<byte> bitSequence, ushort seed, BitPosition bitPosition = BitPosition.One)
        {
            byte shift = lookup[bitPosition];
            byte mask = (byte)~bitPosition;

            foreach (var (point, pixel) in image.RandomDistribution(seed))
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

            EncodeMessageImpl(image, bitSequence, seed, (BitPosition)((byte)bitPosition << 1));
        }
    }
}
