using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.API.Response
{
    public class PagedResponseDto<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }
    }
}
