using Application.Authorization;
using Application.Authorization.Contracts.Request;
using Application.Authorization.Contracts.Response;
using Application.Authorization.Domain;
using Application.Authorization.Interfaces;
using ImageAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ImageAPI.Controllers
{
    public class AccountController : ControllerBase
    {
        public IAccountService AccountService { get; }

        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        [HttpPost(IdentityRoutes.AccountRoutes.Register)]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailResponse), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var authResult = await AccountService.RegisterAsync(request.Username, request.Password);

            return GetResponseFrom(authResult);
        }

        [HttpPost(IdentityRoutes.AccountRoutes.Login)]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailResponse), 400)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var authResult = await AccountService.LoginAsync(request.Username, request.Password);

            return GetResponseFrom(authResult);
        }

        [HttpPost(IdentityRoutes.AccountRoutes.Refresh)]
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailResponse), 400)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authResult = await AccountService.RefreshTokenAsync(request.Token, request.RefreshToken);

            return GetResponseFrom(authResult);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(IdentityRoutes.AccountRoutes.Logout)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Logout()
        {
            await AccountService.LogoutAsync(HttpContext.GetUserId());

            return NoContent();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete(IdentityRoutes.AccountRoutes.Delete)]
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
                return BadRequest(new AuthFailResponse
                {
                    ErrorMessages = authResult.ErrorMessages
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResult.Token,
                RefreshToken = authResult.RefreshToken
            });
        }
    }
}
