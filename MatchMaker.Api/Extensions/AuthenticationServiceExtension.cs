using MatchMaker.Core.Services;
using MatchMaker.Domain.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MatchMaker.Api.Extensions;

public static class AuthenticationServiceExtension
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        try
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
            services.AddSingleton(jwtSettings);

            var jwtOptions = new JwtOptions(jwtSettings);
            services.AddSingleton(jwtOptions);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = jwtOptions.GetTokenValidationParameters();
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
            });

            return services;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to configure JWT-authentication middleware on startup.", ex);
        }
    }
}