using MatchMaker.Domain.Configurations;

namespace MatchMaker.Domain.Extensions;

public static class SmtpClientServiceExtension
{
    public static IServiceCollection AddSmtpServices(this IServiceCollection services, IConfiguration configuration)
    {
        var smtpSettings = new SmtpSettings();
        configuration.GetSection("SmtpSettings").Bind(smtpSettings);

        if (string.IsNullOrEmpty(smtpSettings.FromEmail))
            throw new ArgumentNullException("SmtpFromEmail is missing in config");

        services.AddSingleton(smtpSettings);
        return services;
    }
}
