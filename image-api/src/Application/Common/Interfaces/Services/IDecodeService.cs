using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Services
{
    public interface IDecodeService
    {
        byte[] DecodeMessage(Bitmap image);
    }
}
