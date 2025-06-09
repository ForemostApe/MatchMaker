using MailKit.Net.Smtp;
using MailKit.Security;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace MatchMaker.Core.Services;

public class EmailService(ILogger<EmailService> logger, SmtpSettings smtpSettings, IEmailComposer emailComposer, IEmailTemplateEngine emailTemplateEngine) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly IEmailComposer _emailComposer = emailComposer;
    private readonly IEmailTemplateEngine _emailTemplateEngine = emailTemplateEngine;

    public enum EmailType
    {
        UserCreated,
        PasswordReset,
        GameNotification
    }

    public async Task CreateEmailAsync(string email, EmailType mailType, string? token = null)
    {
        ArgumentNullException.ThrowIfNull(email);

        try
        {
            var (templateName, emailSubject, templateModel) = _emailComposer.Compose(mailType, email, token);

            string mailBody = _emailTemplateEngine.RenderTemplate(templateName, templateModel);

            var mailMessage = new MimeMessage
            {
                From = { new MailboxAddress("MatchMaker", smtpSettings.FromEmail) },
                To = { new MailboxAddress("User", email) },
                Subject = emailSubject,
                Body = new BodyBuilder
                {
                    HtmlBody = _emailTemplateEngine.RenderTemplate(templateName, templateModel),
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
            var socketOptions = smtpSettings.UseTsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None;

            await smtpClient.ConnectAsync(smtpSettings.Host, smtpSettings.Port, socketOptions);

            if (!string.IsNullOrEmpty(smtpSettings.Username))
            {

                await smtpClient.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
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