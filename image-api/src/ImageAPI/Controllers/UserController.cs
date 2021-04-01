using Application.API.Interfaces;
using Application.API.Models;
using Contracts.API;
using Contracts.API.Request;
using Contracts.API.Response;
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
        [ProducesResponseType(typeof(PagedResponseDto<DbUser>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationRequestDto request)
        {
            var (users, total) = await UserRepository.GetAllAsync(request);

            return Ok(new PagedResponseDto<DbUser>
            {
                Data = users,
                Total = total
            });
        }

        [HttpGet(ApiRoutes.UserRoutes.GetById)]
        [ProducesResponseType(typeof(DbUser), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await UserRepository.GetBydIdAsync(id);

            if (user == null)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorMessages = new[] { $"User with '{id}' does not exist." }
                });
            }

            return Ok(user);
        }

        [HttpGet(ApiRoutes.UserRoutes.GetByUsername)]
        [ProducesResponseType(typeof(DbUser), 200)]
        [ProducesResponseType(typeof(ErrorResponseDto), 400)]
        public async Task<IActionResult> GetByName(string username)
        {
            var user = await UserRepository.GetByNameAsync(username);

            if (user == null)
            {
                return BadRequest(new ErrorResponseDto
                {
                    ErrorMessages = new[] { $"User with name '{username}' does not exist." }
                });
            }

            return Ok(user);
        }

        [HttpGet(ApiRoutes.UserRoutes.IsUserNameTake)]
        [ProducesResponseType(typeof(EnvelopDto<bool>), 200)]
        public async Task<IActionResult> IsUserNameTake(string username)
        {
            bool value = await UserRepository.IsUsernameTakenAsync(username);

            return Ok(new EnvelopDto<bool> { Data = value });
        }
    }
}
