using Application.Authorization.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authorization.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task<string> CreateAsync(int userId, string jwtId, DateTime creationDate, DateTime expiryDate);

        Task<RefreshToken> GetByTokenAsync(string refreshToken);

        Task<int> SetUsedAsync(int id);

        Task<int> DeleteForUserAsync(int userId);
    }
}
