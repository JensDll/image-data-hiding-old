using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Domain.UnitTests.Entities
{
    public class RangeRecordTest
    {
        [Fact]
        public void GetMiddle_ShouldReturnTheCorrectValue()
        {
            var r1 = new RangeRecord(0, 10);
            var r2 = new RangeRecord(5, 10);

            Assert.Equal(5, r1.GetMiddle());
            Assert.Equal(7, r2.GetMiddle());
        }

        public static IEnumerable<object[]> TryBisectData()
        {
            yield return new[] { new RangeRecord(0, 10), new RangeRecord(0, 5), new RangeRecord(6, 10) };
            yield return new[] { new RangeRecord(0, 5), new RangeRecord(0, 2), new RangeRecord(3, 5) };
            yield return new[] { new RangeRecord(5, 10), new RangeRecord(5, 7), new RangeRecord(8, 10) };
            yield return new[] { new RangeRecord(9, 10), new RangeRecord(9, 9), new RangeRecord(10, 10) };
        }


        [Theory]
        [MemberData(nameof(TryBisectData))]
        public void TryBisect_ShouldWorkWhenStartAndEndAreDifferent(RangeRecord range, RangeRecord left, RangeRecord right)
        {
            bool success = range.TryBisect(out var ranges);

            Assert.True(success);
            Assert.Equal(left, ranges.Left);
            Assert.Equal(right, ranges.Right);
        }

        [Fact]
        public void TryBisect_ShoulFailWhenStartAndEndAreEqual()
        {
            var range = new RangeRecord(10, 10);

            bool success = range.TryBisect(out _);

            Assert.False(success);
        }
    }
}
