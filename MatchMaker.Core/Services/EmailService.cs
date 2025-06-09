using MailKit.Net.Smtp;
using MailKit.Security;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using MatchMaker.Domain.Enums;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace MatchMaker.Core.Services;

public class EmailService(ILogger<EmailService> logger, IEmailComposer emailComposer, IEmailTemplateEngine emailTemplateEngine, SmtpSettings smtpSettings) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly IEmailComposer _emailComposer = emailComposer;
    private readonly IEmailTemplateEngine _emailTemplateEngine = emailTemplateEngine;
    private readonly SmtpSettings _smtpSettings = smtpSettings;

    public async Task CreateEmailAsync(string email, EmailType mailType, string? token = null)
    {
        ArgumentNullException.ThrowIfNull(email);

        try
        {
            var emailComposition = _emailComposer.Compose(mailType, email, token);

            string mailBody = _emailTemplateEngine.RenderTemplate(emailComposition.TemplateName, emailComposition.TemplateModel);

            var mailMessage = new MimeMessage
            {
                From = { new MailboxAddress("MatchMaker", _smtpSettings.FromEmail) },
                To = { new MailboxAddress("User", email) },
                Subject = emailComposition.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = _emailTemplateEngine.RenderTemplate(emailComposition.TemplateName, emailComposition.TemplateModel),
                    TextBody = "Please enable HTML to view this message."
                }.ToMessageBody()
            };

            await SendEmailAsync(mailMessage);
        }
        catch (Exception ex){
            _logger.LogError(ex, "An unexpected error occurred in EmailService while trying to send mail.");
            throw;
        }
    }

    public async Task SendEmailAsync(MimeMessage mailMessage)
    {
        ArgumentNullException.ThrowIfNull(mailMessage);

        using var smtpClient = new SmtpClient();

        try
        {
            var socketOptions = _smtpSettings.UseTsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;

            await smtpClient.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, socketOptions);

            if (!string.IsNullOrEmpty(_smtpSettings.Username))
            {

                await smtpClient.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            };

            await smtpClient.SendAsync(mailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in EmailService while trying to send email to {userEmailAddress}.", mailMessage.To);
            throw;
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
        }
    }
}