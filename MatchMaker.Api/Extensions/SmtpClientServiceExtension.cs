using MatchMaker.Domain.Configurations;

namespace MatchMaker.Api.Extensions;

public static class SmtpClientServiceExtension
{
    public static IServiceCollection AddSmtpServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

        var clientURL = env.EnvironmentName == "Development" ? configuration.GetValue<string>("FrontendClient:DevelopmentURL") : configuration.GetValue<string>("FrontendClient:ProductionURL");
        if (clientURL == null) throw new InvalidOperationException("Frontend URL is not configured.");

        return services;
    }
}
