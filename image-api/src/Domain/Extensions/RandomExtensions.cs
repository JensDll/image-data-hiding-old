using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class RandomExtensions
    {
        public static int[] Permutation(this Random rand, int start, int count)
        {
            var values = Enumerable.Range(start, count).ToArray();

            for (int i = count - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                int temp = values[i];
                values[i] = values[j];
                values[j] = temp;
            }

            return values;
        }
    }
}
