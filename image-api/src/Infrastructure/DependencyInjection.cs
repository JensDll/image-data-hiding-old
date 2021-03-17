using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Infrastructure.Services;
using Infrastructure.Identity;
using System.Data;
using System.Data.SqlClient;
using Application.Authorization;
using Application.Common.Interfaces.Services;
using Application.Common.Interfaces.Repositories;

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
            services.AddSingleton<IIdentityRepository, IdentityRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddTransient<IDbConnection>(_ => new SqlConnection(configuration.GetConnectionString("IMAGE_DB")));

            // Authorization
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(jwtSettings), jwtSettings);

            services.AddSingleton(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
        }
    }
}
