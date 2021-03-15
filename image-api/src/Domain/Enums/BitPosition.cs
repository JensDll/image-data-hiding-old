using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    [Flags]
    public enum BitPosition
    {
        One     = 0b_0000_0001,
        Two     = 0b_0000_0010,
        Three   = 0b_0000_0100,
        Four    = 0b_0000_1000,
        Five    = 0b_0001_0000,
        Six     = 0b_0010_0000,
        Seven   = 0b_0100_0000,
        Eighth  = 0b_1000_0000
    }
}
