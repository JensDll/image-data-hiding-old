using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.Common.Services
{
    public class EnumerableServiceTest
    {
        public EnumerableServiceTest(IEnumerableService sut)
        {
            Sut = sut;
        }

        public IEnumerableService Sut { get; }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(100, 100)]
        [InlineData(500, 1000)]
        [InlineData(1000, 1000)]
        public void EvenDistribution_ShouldContainTheCorrectItems(int width, int height)
        {
            var image = new Bitmap(width, height);

            var q = Sut.EvenDistribution(image).Select(x => x.Point);

            Assert.DoesNotContain(new Point(0, 0), q);
            Assert.DoesNotContain(new Point(1, 0), q);
            Assert.DoesNotContain(new Point(width - 1, height - 1), q);
            Assert.Equal(width * height - 3, q.Distinct().Count());
        }

        [Fact]
        public void Bitwise_ShouldEnumerateEveryBit()
        {
            var bytes = new byte[] { 0b_0000_1111, 0b_1100_1111 };

            var countZeros = Sut.Bitwise(bytes).Count(bit => bit == 0);
            var countOnes = Sut.Bitwise(bytes).Count(bit => bit == 1);

            Assert.Equal(6, countZeros);
            Assert.Equal(10, countOnes);
        }
    }
}
