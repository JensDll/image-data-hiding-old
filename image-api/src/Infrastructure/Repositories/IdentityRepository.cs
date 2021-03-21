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
using Application.Authorization.Interfaces;
using Application.Authorization.Domain;
using Application.Common.Interfaces;

namespace Infrastructure.Repositories
{
    public class IdentityRepository : RepositoryBase, IIdentityRepository
    {
        public IdentityRepository(IConnectionFactory connectionFactory) : base(connectionFactory)
        { }

        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            using var connection = ConnectionFactory.NewConnection;

            var result = await connection.QueryAsync<string>("spUsers_GetById", param: new { id }, commandType: CommandType.StoredProcedure);

            if (!result.Any())
            {
                return null;
            }

            var user = JsonSerializer.Deserialize<DbUserPassword>(string.Join("", result));

            return new ApplicationUser
            {
                Id = user.Id,
                UserName = user.Username,
                PasswordHash = user.PasswordHash,
                RegistrationDate = user.RegistrationDate,
                DeletionDate = user.DeletionDate
            };
        }

        public async Task<ApplicationUser> GetByNameAsync(string username)
        {
            using var connection = ConnectionFactory.NewConnection;

            var result = await connection.QueryAsync<string>("spUsers_GetByName", param: new { username }, commandType: CommandType.StoredProcedure);

            if (!result.Any())
            {
                return null;
            }

            var user = JsonSerializer.Deserialize<DbUserPassword>(string.Join("", result));

            return new ApplicationUser
            {
                Id = user.Id,
                UserName = user.Username,
                PasswordHash = user.PasswordHash,
                RegistrationDate = user.RegistrationDate,
                DeletionDate = user.DeletionDate
            };
        }

        public async Task<int> CreateAsync(ApplicationUser user)
        {
            using var connection = ConnectionFactory.NewConnection;

            var parameters = new DynamicParameters();
            parameters.Add("username", user.UserName);
            parameters.Add("password", user.PasswordHash);
            parameters.Add("registrationDate", user.RegistrationDate, dbType: DbType.DateTime2);
            parameters.Add("deletionDate", user.DeletionDate, dbType: DbType.DateTime2);

            return await connection.QuerySingleAsync<int>("spUsers_Insert", param: parameters, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteAsync(int id)
        {
            using var connection = ConnectionFactory.NewConnection;

            return await connection.ExecuteAsync("spUsers_Delete", param: new { id }, commandType: CommandType.StoredProcedure);
        }
    }
}
