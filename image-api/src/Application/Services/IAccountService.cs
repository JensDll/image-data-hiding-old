using Domain.Entity;
using Domain.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IAccountService
    {
        Task<AuthResult> RegisterAsync(RegisterUserRequest request);

        Task<AuthResult> LoginAsync(LoginUserRequest request);
    }
}
