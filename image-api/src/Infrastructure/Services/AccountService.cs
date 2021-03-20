using Application.Authorization.Domain;
using Application.Authorization.Interfaces;
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
        private UserManager<ApplicationUser> UserManager { get; }
        private JwtSettings JwtSettings { get; }
        private TokenValidationParameters TokenValidationParameters { get; }
        private IRefreshTokenRepository RefreshTokenRepository { get; }

        public AccountService(UserManager<ApplicationUser> userManager,
            JwtSettings jwtSettings,
            TokenValidationParameters tokenValidationParameters,
            IRefreshTokenRepository refreshTokenRepository)
        {
            UserManager = userManager;
            JwtSettings = jwtSettings;
            TokenValidationParameters = tokenValidationParameters;
            RefreshTokenRepository = refreshTokenRepository;
        }

        public async Task<AuthResult> RegisterAsync(string username, string password)
        {
            var newUser = new ApplicationUser
            {
                UserName = username,
            };

            var identityResult = await UserManager.CreateAsync(newUser, password);

            if (!identityResult.Succeeded)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = identityResult.Errors.Select(e => e.Description),
                };
            }

            _ = Task.Delay(new TimeSpan(0, 15, 0)).ContinueWith(async _ => await DelteAsync(newUser.Id));

            return await GenerateAuthResultAsyncFor(newUser);
        }

        public async Task<AuthResult> LoginAsync(string username, string password)
        {
            var user = await UserManager.FindByNameAsync(username);

            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { $"User with name '{username}' does not exist." }
                };
            }

            bool passwordIsValid = await UserManager.CheckPasswordAsync(user, password);

            if (!passwordIsValid)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "Username/Password combination is incorrect." }
                };
            }

            return await GenerateAuthResultAsyncFor(user);
        }

        public async Task LogoutAsync(int id)
        {
            await RefreshTokenRepository.DeleteForUser(id);
        }

        public async Task<bool> DelteAsync(int id)
        {
            var identityResult = await UserManager.DeleteAsync(new ApplicationUser
            {
                Id = id
            });

            return identityResult.Succeeded;
        }

        public async Task<AuthResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var claimsPrincipal = GetClaimsPrincipalFromToken(token);

            if (claimsPrincipal == null)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "Token is invalid." }
                };
            }

            long expiryDateInSeconds = long.Parse(claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Exp));

            var expiryDate = DateTime.UnixEpoch.AddSeconds(expiryDateInSeconds);

            if (expiryDate > DateTime.UtcNow)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "This token is still valid." }
                };
            }

            var storedToken = await RefreshTokenRepository.GetByToken(refreshToken);

            if (storedToken == null)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "This refresh token does not exist." }
                };
            }

            if (DateTime.UtcNow > storedToken.ExpiryDate)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "This refresh token has expired." }
                };
            }

            if (storedToken.Invalidated)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "This refresh token has been invalidated." }
                };
            }

            if (storedToken.Used)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "This refresh token has been used." }
                };
            }

            string jwtId = claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Jti);
            if (storedToken.JwtId != jwtId)
            {
                return new AuthResult
                {
                    Success = false,
                    ErrorMessages = new[] { "This refresh token does not match the JWT." }
                };
            }

            await RefreshTokenRepository.SetUsed(storedToken.Id);

            var user = await UserManager.FindByNameAsync(claimsPrincipal.FindFirstValue("username"));

            return await GenerateAuthResultAsyncFor(user);
        }

        private ClaimsPrincipal GetClaimsPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                TokenValidationParameters.ValidateLifetime = false;
                var claimsPrincipal = tokenHandler.ValidateToken(token, TokenValidationParameters, out SecurityToken validatedToken);

                return HasValidSecurityAlgorithm(validatedToken) ? claimsPrincipal : null;
            }
            catch
            {
                return null;
            }
            finally
            {
                TokenValidationParameters.ValidateLifetime = true;
            }
        }

        private static bool HasValidSecurityAlgorithm(SecurityToken validatedToken) =>
            validatedToken is JwtSecurityToken securityToken &&
            securityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.CurrentCultureIgnoreCase);


        private async Task<AuthResult> GenerateAuthResultAsyncFor(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("userId", user.Id.ToString()),
                    new Claim("username", user.UserName)
                }),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string refreshToken = await RefreshTokenRepository.CreateAsync(user.Id, token.Id, DateTime.UtcNow, DateTime.UtcNow.AddMonths(1));

            return new AuthResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken
            };
        }
    }
}
