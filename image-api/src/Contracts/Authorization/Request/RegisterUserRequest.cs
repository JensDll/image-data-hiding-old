using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization.Contracts.Request
{
    public record RegisterUserRequest(string Username, string Password);
}
