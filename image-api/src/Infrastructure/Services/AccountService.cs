using Application.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AccountService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthResult> RegisterAsync(RegisterUserRequest request)
        {
            var newUser = new ApplicationUser
            {
                UserName = request.Username,
            };

            var identityResult = await _userManager.CreateAsync(newUser, request.Password);

            if (!identityResult.Succeeded)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = identityResult.Errors.Select(e => e.Description),
                };
            }

            return new AuthResult
            {
                Success = true,
                Token = GenerateTokenFor(newUser)
            };
        }

        public async Task<AuthResult> LoginAsync(LoginUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                return new AuthResult
                {
                    ErrorMessages = new[] { $"User with name '{request.Username}' does not exist." }
                };
            }

            bool passwordIsValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!passwordIsValid)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "Username/Password combination is incorrect." }
                };
            }

            return new AuthResult
            {
                Success = true,
                Token = GenerateTokenFor(user)
            };
        }

        private string GenerateTokenFor(IdentityUser<int> user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
