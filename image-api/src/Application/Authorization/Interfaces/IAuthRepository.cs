using Application.Authorization;
using Application.Authorization.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Authorization.Interfaces
{
    public interface IApplicationUserRepository
    {
        Task<ApplicationUser> GetByIdAsync(int id);

        Task<ApplicationUser> GetByNameAsync(string username);

        Task<int> CreateAsync(ApplicationUser user);

        Task<int> DeleteAsync(int id);
    }
}
