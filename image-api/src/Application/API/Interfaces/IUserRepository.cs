using Application.API.Models;
using Contracts.API.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.API.Interfaces
{
    public interface IUserRepository
    {
        Task<(IEnumerable<DbUser> Users, int Total)> GetAllAsync(PaginationRequestDto request);

        Task<DbUser> GetByNameAsync(string username);

        Task<DbUser> GetBydIdAsync(int id);

        Task<bool> IsUsernameTakenAsync(string username);
    }
}
