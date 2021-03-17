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

        public bool TryBisect(out (RangeRecord Left, RangeRecord Right) ranges)
        {
            int middle = GetMiddle();

            var left = new RangeRecord(Start, middle);
            var right = new RangeRecord(middle + 1, End);

            ranges.Left = left;
            ranges.Right = right;

            return !(Start == End);
        }
    }
}
