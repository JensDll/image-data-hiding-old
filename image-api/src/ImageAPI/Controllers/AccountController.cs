using Application.Authorization;
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
            var authResult = await AccountService.RegisterAsync(request);

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
        [ProducesResponseType(typeof(AuthSuccessResponse), 200)]
        [ProducesResponseType(typeof(AuthFailResponse), 400)]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var authResult = await AccountService.LoginAsync(request);

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
