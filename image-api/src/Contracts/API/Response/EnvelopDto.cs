using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.API.Response
{
    public class EnvelopDto<T>
    {
        public T Data { get; set; }
    }
}
