using Mapster;
using MatchMaker.Core.Facades;
using MatchMaker.Core.Factories;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Profiles;
using MatchMaker.Core.Services;
using MatchMaker.Core.Utilities;
using MatchMaker.Data.Interfaces;
using MatchMaker.Data.Repositories;
using MatchMaker.Domain.Configurations;

namespace MatchMaker.Api.Extensions;

public static class CoreServiceExtension
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
    {

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(UserMappingProfile).Assembly);
        services.AddMapster();

        services.AddHttpContextAccessor();

        services.AddScoped<IUserServiceFacade, UserServiceFacade>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepo, UserRepo>();

        services.AddScoped<IAuthServiceFacade, AuthServiceFacade>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<ICookieFactory, CookieFactory>();

        services.AddScoped<ISessionManager, SessionManager>();

        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.AddTransient<IEmailService, EmailService>();

        var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedOrigins ?? new[] { "http://localhost:5173" })
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        services.AddSingleton(config);

        services.AddDistributedMemoryCache(); //Check out how to use this properly later on.

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(30);
        });

        services.AddHealthChecks();

        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddConsole();
            config.AddDebug();
            //Add additional logging-outputs other than console and debug here if needed.
        });

        return services;
    }
}
