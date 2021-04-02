using Application.Authorization.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization.Interfaces
{
    public interface IAccountService
    {
        Task<AuthResult> RegisterAsync(string username, string password);

        Task<AuthResult> LoginAsync(string username, string password);

        Task<AuthResult> RefreshTokenAsync(string token, string refreshToken);

        Task LogoutAsync(int id);

        Task<bool> DelteAsync(int id);
    }
}
