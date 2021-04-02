using Application.API.Interfaces;
using Application.API.Models;
using Application.Authorization.Interfaces;
using Application.Data;
using Contracts.API.Request;
using Contracts.API.Response;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.API.Repositories
{
    internal class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly IApplicationUserRepository applicationUserRepository;

        public UserRepository(IConnectionFactory connectionFactory, IApplicationUserRepository applicationUserRepository) : base(connectionFactory)
        {
            this.applicationUserRepository = applicationUserRepository;
        }

        public async Task<(IEnumerable<UserDto> Users, int Total)> GetAllAsync(PaginationRequestDto request)
        {
            using var connection = ConnectionFactory.NewConnection;

            (int skip, int take) = GetSkipTake(request);

            var parameter = new DynamicParameters();
            parameter.Add("skip", skip);
            parameter.Add("take", take);
            parameter.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = await connection.QueryAsync<string>("spUsers_GetAll", param: parameter, commandType: CommandType.StoredProcedure);

            int total = parameter.Get<int>("total");

            if (!result.Any())
            {
                return (Enumerable.Empty<UserDto>(), total);
            }

            var dbUsers = JsonSerializer.Deserialize<IEnumerable<DbUser>>(string.Join("", result));
            var users = dbUsers.Select(dbUser => new UserDto
            {
                Id = dbUser.Id,
                Username = dbUser.Username,
                TimeUntilDeletion = GetTimeUntilDeletion(dbUser.DeletionDate)
            });

            return (users, total);
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            var applicationUser = await applicationUserRepository.GetByNameAsync(username);

            return applicationUser != null;
        }

        public async Task<UserDto> GetBydIdAsync(int id)
        {
            var applicationUser = await applicationUserRepository.GetByIdAsync(id);

            return applicationUser == null ? null : new UserDto
            {
                Id = applicationUser.Id,
                Username = applicationUser.UserName,
                TimeUntilDeletion = GetTimeUntilDeletion(applicationUser.DeletionDate)
            };
        }

        public async Task<UserDto> GetByNameAsync(string username)
        {
            var applicationUser = await applicationUserRepository.GetByNameAsync(username);

            return applicationUser == null ? null : new UserDto
            {
                Id = applicationUser.Id,
                Username = applicationUser.UserName,
                TimeUntilDeletion = GetTimeUntilDeletion(applicationUser.DeletionDate)
            };
        }

        private long GetTimeUntilDeletion(DateTime deletionDate)
        {
            long ms = (long)(deletionDate - DateTime.Now).TotalMilliseconds;

            return ms > 0 ? ms : 0;
        }

    }
}
