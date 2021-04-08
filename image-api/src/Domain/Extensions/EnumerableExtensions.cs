using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<byte> Bitwise(this byte[] bytes)
        {
            var masks = new byte[] { 1, 2, 4, 8, 16, 32, 64, 128 };

            foreach (byte b in bytes)
            {
                for (int i = 7; i >= 0; i--)
                {
                    yield return (byte)((b & masks[i]) >> i);
                }
            }
        }
    }
}
