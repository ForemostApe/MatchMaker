using MatchMaker.Domain.Configurations;

namespace MatchMaker.Api.Extensions;

public static class SmtpClientServiceExtension
{
    public static IServiceCollection AddSmtpServices(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = new SmtpSettings();
        configuration.GetSection("SmtpSettings").Bind(smtpSettings);

        if(string.IsNullOrWhiteSpace(smtpSettings.FromEmail)) throw new InvalidOperationException("SmtpFromEmail is missing in configuration.");

        services.AddSingleton(smtpSettings);
        return services;
    }
}
