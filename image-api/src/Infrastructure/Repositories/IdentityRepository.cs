using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Text.Json;
using Application.Common.Models;
using Application.Authorization;
using Application.Common.Interfaces.Repositories;

namespace Infrastructure.Repositories
{
    public class IdentityRepository : RepositoryBase, IIdentityRepository
    {
        public IdentityRepository(IDbConnection connection) : base(connection)
        { }

        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            var result = await Connection.QueryAsync<string>("spUsers_GetById", param: new { id }, commandType: CommandType.StoredProcedure);

            if (!result.Any())
            {
                return null;
            }

            var user = JsonSerializer.Deserialize<DbUserPassword>(string.Join("", result));

            return new ApplicationUser
            {
                Id = user.Id,
                UserName = user.Username,
                PasswordHash = user.PasswordHash
            };
        }

        public async Task<ApplicationUser> GetByNameAsync(string username)
        {
            var result = await Connection.QueryAsync<string>("spUsers_GetByName", param: new { username }, commandType: CommandType.StoredProcedure);

            if (!result.Any())
            {
                return null;
            }

            var user = JsonSerializer.Deserialize<DbUserPassword>(string.Join("", result));

            return new ApplicationUser
            {
                Id = user.Id,
                UserName = user.Username,
                PasswordHash = user.PasswordHash
            };
        }

        public async Task<int> CreateAsync(string username, string password)
        {
            return await Connection.QuerySingleAsync<int>("spUsers_Insert", param: new { username, password }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await Connection.ExecuteAsync("spUser_Delete", param: new { id }, commandType: CommandType.StoredProcedure);
        }
    }
}
