using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Contracts.Request
{
    public record EncodeRequest(int UserId, string Username, string Message);
}
