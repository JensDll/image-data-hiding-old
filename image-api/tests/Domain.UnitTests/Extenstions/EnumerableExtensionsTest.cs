using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.UnitTests.Extenstions
{
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void Bitwise_ShouldEnumerateEveryBit()
        {
            var bytes = new byte[] { 0b_0000_1111, 0b_1100_1111 };

            var countZeros = bytes.Bitwise().Count(bit => bit == 0);
            var countOnes = bytes.Bitwise().Count(bit => bit == 1);

            Assert.Equal(6, countZeros);
            Assert.Equal(10, countOnes);
        }
    }
}
