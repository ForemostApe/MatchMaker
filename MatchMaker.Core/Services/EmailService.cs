using MailKit.Net.Smtp;
using MailKit.Security;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MatchMaker.Core.Services;

public class EmailService(ILogger<EmailService> logger, SmtpSettings smtpSettings, ILinkFactory linkFactory, IEmailTemplateEngine emailTemplateEngine, ClientSettings clientSettings) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly SmtpSettings _smtpSettings = smtpSettings;
    private readonly ILinkFactory _linkFactory = linkFactory;
    private readonly IEmailTemplateEngine _emailTemplateEngine = emailTemplateEngine;
    private readonly ClientSettings _clientSettings = clientSettings;

    public enum EmailType
    {
        UserCreated,
        PasswordReset,
        GameNotification
    }

    public async Task CreateEmailAsync(string email, EmailType mailType, string? token = null)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentNullException(nameof(email), "Recipient email cannot be null or empty.");
        }

        try
        {
            string templateName;
            string emailSubject;
            object templateModel;

            switch (mailType)
            {
                case EmailType.UserCreated:
                    templateName = "UserCreatedTemplate";
                    emailSubject = "Ditt MatchMaker-konto har skapats.";
                    templateModel = new { verification_link = !string.IsNullOrEmpty(token) ? _linkFactory.CreateVerificationLink(token) : throw new ArgumentNullException("Token is null when trying to create verification-link.") };
                    break;

                case EmailType.PasswordReset:
                    templateName = "PasswordResetTemplate";
                    emailSubject = "Begäran att återställa MatchMaker-lösenord.";
                    templateModel = new { resetPassword_link = !string.IsNullOrEmpty(email) ? _linkFactory.CreateResetPasswordLink(email!) : throw new ArgumentNullException("Email is null when trying create reset password-link.") };
                    break;

                case EmailType.GameNotification:
                    templateName = "GameNotificationTemplate";
                    emailSubject = "En planerad match inväntar bedömning.";
                    templateModel = new { login_link = !string.IsNullOrEmpty(email) ? _clientSettings.BaseURL : throw new ArgumentNullException("Email is null when trying create reset password-link.") };
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mailType));
            }

            string mailBody = _emailTemplateEngine.RenderTemplate(templateName, templateModel);

            var mailMessage = new MimeMessage
            {
                From = { new MailboxAddress("MatchMaker", _smtpSettings.FromEmail) },
                To = { new MailboxAddress("User", email) },
                Subject = emailSubject,
                Body = new BodyBuilder
                {
                    HtmlBody = _emailTemplateEngine.RenderTemplate(templateName, templateModel),
                    TextBody = "Please enable HTML to view this message."
                }.ToMessageBody()
            };

            _logger.LogInformation("UserCreated email content successfully created.");
            await SendEmailAsync(mailMessage);
        }
        catch (Exception ex){
            _logger.LogError(ex, "An unexpected error occurred while trying to send mail.");
            throw new Exception($"An unexpected error occurred while trying to send mail. {ex.Message}");
        }
    }

    public async Task SendEmailAsync(MimeMessage mailMessage)
    {
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
            _logger.LogError(ex, "Error while trying to send email to {userEmailAddress}.", mailMessage.To);
            throw new ApplicationException("An unexpected error occurred while trying to send email. Please try again later.", ex);
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
        }
    }
}