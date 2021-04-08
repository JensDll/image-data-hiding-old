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
        public void RandomDistribution_ShouldContainTheCorrectItems(int width, int height)
        {
            var image = new Bitmap(width, height);

            var q = image.RandomDistribution().Select(x => x.Point);

            Assert.DoesNotContain(new Point(0, 0), q);
            Assert.DoesNotContain(new Point(1, 0), q);
            Assert.DoesNotContain(new Point(width - 1, height - 1), q);
            Assert.Equal(width * height - 3, q.Distinct().Count());
        }
    }
}
