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
        public Task<string> CreateAsync(int userId, string jwtId, DateTime creationDate, DateTime expiryDate);

        public Task<RefreshToken> GetByTokenAsync(string refreshToken);

        public Task<int> SetUsedAsync(int id);

        public Task<int> DeleteForUserAsync(int userId);
    }
}
