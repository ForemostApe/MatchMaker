using MailKit.Net.Smtp;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Services;
using MatchMaker.Domain.Configurations;
using Microsoft.Extensions.Options;

namespace MatchMaker.Api.Extensions;

public static class SmtpClientServiceExtension
{
    public static IServiceCollection AddSmtpServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            
        services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
        services.AddSingleton<SmtpClient>(options =>
            {
                var smtpSettings = options.GetRequiredService<IOptions<SmtpSettings>>().Value;
                var smtpClient = new SmtpClient();
                smtpClient.Connect(smtpSettings.Host, smtpSettings.Port, smtpSettings.EnableSsl);
                smtpClient.Authenticate(smtpSettings.Username, smtpSettings.Password);

                return smtpClient;
            });

        return services;
    }
}
