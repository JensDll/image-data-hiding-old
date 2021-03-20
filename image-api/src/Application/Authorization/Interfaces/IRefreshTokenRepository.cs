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

        public Task<RefreshToken> GetByToken(string refreshToken);

        public Task<int> SetUsed(int id);

        public Task<int> DeleteForUser(int userId);
    }
}
