using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization
{
    public class AuthResult
    {
        public bool Success { get; set; }

        public string Token { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
