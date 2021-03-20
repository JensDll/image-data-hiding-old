using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Contracts.Request;
using Domain.Contracts.Response;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet(ApiRoutes.UserRoutes.IsUserNameTake)]
        [ProducesResponseType(typeof(Envelop<bool>), 200)]
        public async Task<IActionResult> GetAll(string username)
        {
            bool value = await UserRepository.IsUsernameTakenAsync(username);

            return Ok(new Envelop<bool> { Data = value });
        }
    }
}
