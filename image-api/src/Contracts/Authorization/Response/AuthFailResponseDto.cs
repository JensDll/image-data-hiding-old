using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Authorization.Response
{
    public class AuthFailResponseDto
    {
        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
