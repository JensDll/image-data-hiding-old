using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Response
{
    public class ErrorResponse
    {
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
