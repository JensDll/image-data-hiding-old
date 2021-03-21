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

        public static IEnumerable<object[]> TryBisectDataSuccess()
        {
            yield return new object[] { new RangeRecord(0, 10), new RangeRecord(0, 4), 5, new RangeRecord(6, 10) };
            yield return new object[] { new RangeRecord(0, 20), new RangeRecord(0, 9), 10, new RangeRecord(11, 20) };
            yield return new object[] { new RangeRecord(0, 5), new RangeRecord(0, 1), 2, new RangeRecord(3, 5) };
            yield return new object[] { new RangeRecord(5, 10), new RangeRecord(5, 6), 7, new RangeRecord(8, 10) };
        }

        public static IEnumerable<object[]> TryBisectDataFail()
        {
            yield return new[] { new RangeRecord(0, 0) };
            yield return new[] { new RangeRecord(10, 10) };
            yield return new[] { new RangeRecord(0, 1) };
            yield return new[] { new RangeRecord(10, 11) };
        }

        [Theory]
        [MemberData(nameof(TryBisectDataSuccess))]
        public void TryBisect_ShouldWorkWhenRangeLengthIsGreaterThanOrEqualToThree(RangeRecord range, RangeRecord left, int middle, RangeRecord right)
        {
            bool success = range.TryBisect(out var ranges);

            Assert.True(success);
            Assert.Equal(left, ranges.Left);
            Assert.Equal(middle, ranges.middle);
            Assert.Equal(right, ranges.Right);
        }

        [Theory]
        [MemberData(nameof(TryBisectDataFail))]
        public void TryBisect_ShoulFailWhenRangeLengthIsLessThanThree(RangeRecord range)
        {
            bool success = range.TryBisect(out _);

            Assert.False(success);
        }
    }
}
