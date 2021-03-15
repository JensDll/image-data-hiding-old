using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface IEnumerableService
    {
        IEnumerable<byte> Bitwise(byte[] bytes);

        IEnumerable<(Point, Pixel)> EvenDistribution(Bitmap image);
    }
}
