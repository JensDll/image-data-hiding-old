using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Services;
using Application.Common.Models;
using Domain.Contracts.Request;
using Domain.Contracts.Response;
using Domain.Enums;
using Domain.Exceptions;
using ImageAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private IUserRepository UserRepository { get; }

        public UserController(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        [HttpGet(ApiRoutes.UserRoutes.GetAll)]
        [ProducesResponseType(typeof(PagedResponse<DbUser>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequest request)
        {
            var (users, total) = await UserRepository.GetAllAsync(request);

            return Ok(new PagedResponse<DbUser>
            {
                Data = users,
                Total = total
            });
        }
    }
}
