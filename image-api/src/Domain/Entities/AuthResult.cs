using Domain.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class AuthResult : ErrorMessageBase
    {
        public bool Success { get; set; }

        public string Token { get; set; }
    }
}
