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

        [HttpGet(ApiRoutes.UserRoutes.GetById)]
        [ProducesResponseType(typeof(DbUser), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await UserRepository.GetBydIdAsync(id);

            if (user == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessages = new[] { $"User with '{id}' does not exist." }
                });
            }

            return Ok(user);
        }

        [HttpGet(ApiRoutes.UserRoutes.GetByUsername)]
        [ProducesResponseType(typeof(DbUser), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> GetByName(string username)
        {
            var user = await UserRepository.GetByNameAsync(username);

            if (user == null)
            {
                return BadRequest(new ErrorResponse
                {
                    ErrorMessages = new[] { $"User with name '{username}' does not exist." }
                });
            }

            return Ok(user);
        }

        [HttpGet(ApiRoutes.UserRoutes.IsUserNameTake)]
        [ProducesResponseType(typeof(Envelop<bool>), 200)]
        public async Task<IActionResult> IsUserNameTake(string username)
        {
            bool value = await UserRepository.IsUsernameTakenAsync(username);

            return Ok(new Envelop<bool> { Data = value });
        }
    }
}
