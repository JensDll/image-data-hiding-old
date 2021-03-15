﻿using Application.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost(IdentityRoutes.AccountRoutes.Register)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var authResult = await _accountService.RegisterAsync(request);

            if (!authResult.Success)
            {
                return BadRequest(new AuthFailResponse
                {
                    ErrorMessages = authResult.ErrorMessages
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token,
            });
        }

        [HttpPost(IdentityRoutes.AccountRoutes.Login)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var authResult = await _accountService.LoginAsync(request);

            if (!authResult.Success)
            {
                return BadRequest(new AuthFailResponse
                {
                    ErrorMessages = authResult.ErrorMessages
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token
            });
        }
    }
}
