using Application.Authorization;
using Application.Authorization.Domain;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Contracts.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Application.Authorization.Interfaces
{
    public interface IIdentityRepository
    {
        Task<ApplicationUser> GetByIdAsync(int id);

        Task<ApplicationUser> GetByNameAsync(string username);

        Task<int> CreateAsync(ApplicationUser user);

        Task<int> DeleteAsync(int id);
    }
}
