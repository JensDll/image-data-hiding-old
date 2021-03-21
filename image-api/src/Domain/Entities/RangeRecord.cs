using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record RangeRecord
    {
        public int Start { get; }

        public int End { get; }

        public RangeRecord(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int GetLength() => End - Start;

        public int GetMiddle() => Start + (GetLength() >> 1);

        public bool TryBisect(out (RangeRecord Left, int middle, RangeRecord Right) ranges)
        {
            ranges.middle = GetMiddle();
            ranges.Left = new (Start, ranges.middle - 1);
            ranges.Right = new (ranges.middle + 1, End);

            return GetLength() >= 2;
        }
    }
}
