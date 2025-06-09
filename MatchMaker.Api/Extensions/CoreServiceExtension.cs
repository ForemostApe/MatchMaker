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
    public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(UserMappingProfile).Assembly);
        services.AddMapster();

        services.Configure<ClientSettings>(configuration.GetSection("FrontendClient"));
        var clientSettings = configuration.GetSection("FrontendClient").Get<ClientSettings>() ?? throw new ArgumentNullException(nameof(configuration), "Couldn't get BaseURL settings.");
                
        services.AddSingleton(clientSettings);

        services.AddHttpContextAccessor();

        services.AddScoped<IUserServiceFacade, UserServiceFacade>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepo, UserRepo>();

        services.AddScoped<IAuthServiceFacade, AuthServiceFacade>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<ITokenService, TokenService>();

        services.AddScoped<ICookieFactory, CookieFactory>();

        services.AddScoped<ISessionManager, SessionManager>();

        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IEmailComposer, EmailComposer>();
        services.AddSingleton<IEmailTemplateEngine, EmailTemplateEngine>();
        
        services.AddScoped<ILinkFactory>(_ => new LinkFactory(_.GetRequiredService<ILogger<LinkFactory>>(), clientSettings.BaseURL));

        services.AddScoped<ITeamServiceFacade, TeamServiceFacade>();
        services.AddScoped<ITeamService, TeamService>();
        services.AddScoped<ITeamRepo, TeamRepo>();

        services.AddScoped<IGameServiceFacade, GameServiceFacade>();
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IGameRepo, GameRepo>();

        services.AddSingleton(config);

        services.AddControllers();

        services.AddEndpointsApiExplorer();

        services.AddDistributedMemoryCache(); 
        services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.IdleTimeout = TimeSpan.FromMinutes(30);
        });

        services.AddHealthChecks();

        //Add additional logging-outputs other than console and debug here if needed.
        services.AddLogging(config =>
        {
            config.ClearProviders();
            config.AddConsole();
            config.AddDebug();
        });

        return services;
    }
}
