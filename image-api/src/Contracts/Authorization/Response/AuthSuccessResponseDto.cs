using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Authorization.Response
{
    public class AuthSuccessResponseDto
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
