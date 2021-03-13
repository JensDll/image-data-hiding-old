﻿using Application.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetByIdAsync(int id);

        Task<ApplicationUser> GetByNameAsync(string username);

        Task<int> CreateAsync(string username, string password);

        Task<int> DeleteAsync(int id);
    }
}
