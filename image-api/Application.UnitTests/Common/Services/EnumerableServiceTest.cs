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

        [Fact]
        public void EvenDistribution_ShouldContainAllItems()
        {
            var image = new Bitmap(10, 10);

            int count = Sut.EvenDistribution(image).Count();

            Assert.Equal(10 * 10, count);
        }

        [Fact]
        public void EvenDistribution_ShouldNotHaveDuplicates()
        {
            var image = new Bitmap(100, 100);

            int distinctCount = Sut.EvenDistribution(image).Select(x => x.Point).Distinct().Count();

            Assert.Equal(100 * 100, distinctCount);
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
