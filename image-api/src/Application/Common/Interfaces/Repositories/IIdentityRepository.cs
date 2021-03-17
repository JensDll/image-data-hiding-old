using Application.Authorization;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Common.Interfaces.Repositories
{
    public interface IIdentityRepository
    {
        Task<ApplicationUser> GetByIdAsync(int id);

        Task<ApplicationUser> GetByNameAsync(string username);

        Task<int> CreateAsync(string username, string password);

        Task<int> DeleteAsync(int id);
    }
}
