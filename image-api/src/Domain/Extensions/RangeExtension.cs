using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class RangeExtension
    {
        public static bool TryBisect(this Range range, out (Range Left, Range Right) ranges)
        {
            int middle = range.GetMiddle();
            int start = range.Start.Value;
            int end = range.End.Value;

            int middleValue = start + middle;

            var left = start..middleValue;
            var right = (middleValue + 1)..end;

            ranges.Left = left;
            ranges.Right = right;

            return !(start == end);
        }

        public static int GetLength(this Range range)
        {
            return range.End.Value - range.Start.Value;
        }

        private static int GetMiddle(this Range range)
        {
            int length = range.GetLength();

            return length >>= 1;
        }
    }
}
