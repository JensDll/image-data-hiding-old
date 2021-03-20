using Application.Authorization.Domain;
using Application.Authorization.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : RepositoryBase, IRefreshTokenRepository
    {
        public RefreshTokenRepository(IDbConnection connection) : base(connection)
        { }

        public async Task<string> CreateAsync(int userId, string jwtId, DateTime creationDate, DateTime expiryDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add(nameof(userId), userId);
            parameters.Add(nameof(jwtId), jwtId);
            parameters.Add(nameof(creationDate), creationDate, dbType: DbType.DateTime2);
            parameters.Add(nameof(expiryDate), expiryDate, dbType: DbType.DateTime2);

            string token = await Connection.QuerySingleAsync<string>("spRefreshTokens_Insert", param: parameters, commandType: CommandType.StoredProcedure);

            return token;
        }

        public async Task<RefreshToken> GetByToken(string refreshToken)
        {
            var result = await Connection.QueryAsync<string>("spRefreshTokens_GetByToken", param: new { token = refreshToken }, commandType: CommandType.StoredProcedure);

            if (!result.Any())
            {
                return null;
            }

            return JsonSerializer.Deserialize<RefreshToken>(string.Join("", result));
        }

        public async Task<int> SetUsed(int id)
        {
            return await Connection.ExecuteAsync("spRefreshTokens_SetUsed", param: new { id }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteForUser(int userId)
        {
            return await Connection.ExecuteAsync("spRefreshTokens_DeleteForUser", param: new { userId }, commandType: CommandType.StoredProcedure);
        }
    }
}
