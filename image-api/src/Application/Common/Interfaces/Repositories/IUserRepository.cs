using Application.Common.Models;
using Domain.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<(IEnumerable<DbUser> Users, int Total)> GetAllAsync(PaginationRequest request);
    }
}
