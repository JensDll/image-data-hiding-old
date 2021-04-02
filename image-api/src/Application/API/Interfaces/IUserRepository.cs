using Contracts.API.Request;
using Contracts.API.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.API.Interfaces
{
    public interface IUserRepository
    {
        Task<(IEnumerable<UserDto> Users, int Total)> GetAllAsync(PaginationRequestDto request);

        Task<UserDto> GetByNameAsync(string username);

        Task<UserDto> GetBydIdAsync(int id);

        Task<bool> IsUsernameTakenAsync(string username);
    }
}
