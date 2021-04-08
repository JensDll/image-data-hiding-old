using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.UnitTests.Extenstions
{
    public class BitmapExtensionsTest
    {
        [Theory]
        [InlineData(500, 1000)]
        [InlineData(100, 100)]
        [InlineData(10, 10)]
        public void RandomDistribution_ShouldContainEveryPixelWithMessageInformation(int width, int height)
        {
            var image = new Bitmap(width, height);

            var q = image.RandomDistribution(420).Select(x => x.Point);

            Assert.DoesNotContain(new Point(0, 0), q);
            Assert.DoesNotContain(new Point(1, 0), q);
            Assert.DoesNotContain(new Point(width - 2, height - 1), q);
            Assert.DoesNotContain(new Point(width - 1, height - 1), q);
            Assert.Equal(width * height - 4, q.Distinct().Count());
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(1234567)]
        [InlineData(123456)]
        [InlineData(12345)]
        [InlineData(1234)]
        [InlineData(123)]
        [InlineData(12)]
        [InlineData(1)]
        public void EncodeDecode_MessageLength(int messageLength)
        {
            var image = new Bitmap(10, 10);
            image.EncodeMessageLength(messageLength);
            Assert.Equal(messageLength, image.DecodeMessageLength());
        }

        [Theory]
        [InlineData(ushort.MaxValue)]
        [InlineData(420)]
        [InlineData(42)]
        [InlineData(100)]
        public void EncodeDecode_Seed(ushort seed)
        {
            var image = new Bitmap(10, 10);
            image.EncodeSeed(seed);
            Assert.Equal(seed, image.DecodeSeed());
        }
    }
}
