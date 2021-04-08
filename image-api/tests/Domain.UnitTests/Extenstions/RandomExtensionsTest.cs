using Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.UnitTests.Extenstions
{
    public class RandomExtensionsTest
    {
        [Theory]
        [InlineData(1000000)]
        [InlineData(100000)]
        [InlineData(10000)]
        [InlineData(1000)]
        [InlineData(100)]
        [InlineData(10)]
        public void Permution_ShouldBeDeterministic(int count)
        {
            var rand1 = new Random(42);
            var rand2 = new Random(42);

            var perumation1 = rand1.Permutation(0, count);
            var perumation2 = rand2.Permutation(0, count);

            Assert.Equal(perumation1, perumation2);
        }
    }
}
