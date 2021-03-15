using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Text.Json;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Authorization;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            var result = await _connection.QueryAsync<string>("spUsers_GetById", param: new { id }, commandType: CommandType.StoredProcedure);

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
            var result = await _connection.QueryAsync<string>("spUsers_GetByName", param: new { username }, commandType: CommandType.StoredProcedure);

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
            return await _connection.QuerySingleAsync<int>("spUsers_Insert", param: new { username, password }, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(int id)
        {
            return await _connection.ExecuteAsync("spUser_Delete", param: new { id }, commandType: CommandType.StoredProcedure);
        }
    }
}
