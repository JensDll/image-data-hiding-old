using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization.Contracts.Response
{
    public class AuthFailResponse
    {
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
