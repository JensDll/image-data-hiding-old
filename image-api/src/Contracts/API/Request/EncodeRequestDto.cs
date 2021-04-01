using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.API.Request
{
    public record EncodeRequestDto(int UserId, string Username, string Message);
}
