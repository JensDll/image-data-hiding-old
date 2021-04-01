using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Contracts.Response
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }
    }
}
