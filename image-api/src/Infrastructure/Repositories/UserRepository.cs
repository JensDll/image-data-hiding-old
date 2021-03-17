using Application.Common.Interfaces.Repositories;
using Application.Common.Models;
using Dapper;
using Domain.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(IDbConnection connection) : base(connection)
        { }

        public async Task<(IEnumerable<DbUser> Users, int Total)> GetAllAsync(PaginationRequest request)
        {
            (int skip, int take) = GetSkipTake(request);

            var parameter = new DynamicParameters();
            parameter.Add("skip", skip);
            parameter.Add("take", take);
            parameter.Add("total", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var result = await Connection.QueryAsync<string>("spUsers_GetAll", param: parameter, commandType: CommandType.StoredProcedure);

            int total = parameter.Get<int>("total");

            if (!result.Any())
            {
                return (Enumerable.Empty<DbUser>(), total);
            }

            var users = JsonSerializer.Deserialize<IEnumerable<DbUser>>(string.Join("", result));

            return (users, total);
        }
    }
}
