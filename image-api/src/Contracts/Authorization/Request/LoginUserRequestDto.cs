using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Authorization.Request
{
    public record LoginUserRequestDto(string Username, string Password);
}
