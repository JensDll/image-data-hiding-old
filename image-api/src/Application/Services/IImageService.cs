using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IImageService
    {
        bool WriteMessage(Bitmap image, byte[] message);

        byte[] ReadMessage(Bitmap image);
    }
}
