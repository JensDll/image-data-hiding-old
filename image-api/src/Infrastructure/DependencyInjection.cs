using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Application.Authorization.Interfaces;
using Application.Authorization.Domain;
using System.Security.Cryptography;
using System;
using Infrastructure.Authorization.Identity;
using Infrastructure.Authorization.Services;
using Infrastructure.API.Services;
using Infrastructure.Authorization.Repositories;
using Infrastructure.API.Repositories;
using Application.Data;
using Application.API.Interfaces;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Identity
            services.AddIdentityCore<ApplicationUser>().AddDefaultTokenProviders();
            services.AddTransient<IUserStore<ApplicationUser>, CustomUserStore>();

            // Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddSingleton<IEnumerableService, EnumerableService>();
            services.AddSingleton<IEncodeService, EncodeService>();
            services.AddSingleton<IDecodeService, DecodeService>();

            // Data Access
            services.AddSingleton<IConnectionFactory>(new ConnectionFactory(configuration.GetConnectionString("IMAGE_DB")));
            services.AddSingleton<IAuthRepository, AuthRepository>();
            services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();

            if (configuration != null)
            {
                // Authorization
                var csrng = new RNGCryptoServiceProvider();
                var secret = new byte[32];
                csrng.GetBytes(secret);

                var jwtSettings = new JwtSettings
                {
                    Secret = Encoding.ASCII.GetString(secret)
                };

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                services.AddSingleton(jwtSettings);
                services.AddSingleton(tokenValidationParameters);

                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = tokenValidationParameters;
                });
            }
        }
    }
}
