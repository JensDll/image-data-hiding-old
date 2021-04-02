using Application.Authorization;
using Application.Authorization.Domain;
using Application.Authorization.Interfaces;
using Contracts.Authorization;
using Contracts.Authorization.Request;
using Contracts.Authorization.Response;
using ImageAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    public class AuthController : ControllerBase
    {
        public IAccountService AccountService { get; }

        public AuthController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        [HttpPost(AuthRoutes.AccountRoutes.Register)]
        [ProducesResponseType(typeof(AuthSuccessResponseDto), 200)]
        [ProducesResponseType(typeof(AuthFailResponseDto), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto request)
        {
            var authResult = await AccountService.RegisterAsync(request.Username, request.Password);

            return GetResponseFrom(authResult);
        }

        [HttpPost(AuthRoutes.AccountRoutes.Login)]
        [ProducesResponseType(typeof(AuthSuccessResponseDto), 200)]
        [ProducesResponseType(typeof(AuthFailResponseDto), 400)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequestDto request)
        {
            var authResult = await AccountService.LoginAsync(request.Username, request.Password);

            return GetResponseFrom(authResult);
        }

        [HttpPost(AuthRoutes.AccountRoutes.Refresh)]
        [ProducesResponseType(typeof(AuthSuccessResponseDto), 200)]
        [ProducesResponseType(typeof(AuthFailResponseDto), 400)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto request)
        {
            var authResult = await AccountService.RefreshTokenAsync(request.Token, request.RefreshToken);

            return GetResponseFrom(authResult);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(AuthRoutes.AccountRoutes.Logout)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Logout()
        {
            await AccountService.LogoutAsync(HttpContext.GetUserId());

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(AuthRoutes.AccountRoutes.Delete)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Delete()
        {
            await AccountService.DelteAsync(HttpContext.GetUserId());

            return NoContent();
        }

        private IActionResult GetResponseFrom(AuthResult authResult)
        {
            if (!authResult.Success)
            {
                return BadRequest(new AuthFailResponseDto
                {
                    ErrorMessages = authResult.ErrorMessages
                });
            }

            return Ok(new AuthSuccessResponseDto
            {
                Token = authResult.Token,
                RefreshToken = authResult.RefreshToken
            });
        }
    }
}
