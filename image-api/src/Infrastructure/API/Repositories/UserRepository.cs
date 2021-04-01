using Application.API.Interfaces;
using Application.API.Models;
using Application.Authorization.Interfaces;
using Application.Data;
using Contracts.API.Request;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.API.Repositories
{
    internal class UserRepository : RepositoryBase, IUserRepository
    {
        private readonly IAuthRepository authRepository;

        public UserRepository(IConnectionFactory connectionFactory, IAuthRepository authRepository) : base(connectionFactory)
        {
            this.authRepository = authRepository;
        }

        public async Task<(IEnumerable<DbUser> Users, int Total)> GetAllAsync(PaginationRequestDto request)
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
                return (Enumerable.Empty<DbUser>(), total);
            }

            var users = JsonSerializer.Deserialize<IEnumerable<DbUser>>(string.Join("", result));

            return (users, total);
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            var user = await authRepository.GetByNameAsync(username);

            return user is not null;
        }

        public async Task<DbUser> GetBydIdAsync(int id)
        {
            var user = await authRepository.GetByIdAsync(id);

            return user is null ? null : new DbUser
            {
                Id = user.Id,
                Username = user.UserName,
                DeletionDate = user.DeletionDate
            };
        }

        public async Task<DbUser> GetByNameAsync(string username)
        {
            var user = await authRepository.GetByNameAsync(username);

            return user is null ? null : new DbUser
            {
                Id = user.Id,
                Username = user.UserName,
                DeletionDate = user.DeletionDate
            };
        }
    }
}
