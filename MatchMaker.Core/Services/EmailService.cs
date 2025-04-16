using MailKit.Net.Smtp;
using MailKit.Security;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Text;

namespace MatchMaker.Core.Services;

public class EmailService(ILogger<EmailService> logger, SmtpSettings smtpSettings) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly SmtpSettings _smtpSettings = smtpSettings;

    public enum EmailType
    {
        UserCreated,
        PasswordReset,
        LoginLink,
    }

    public async Task CreateEmailAsync(string email, EmailType mailType)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email), "Recipient email cannot be null or empty.");
            }

                byte[] templateBytes = mailType switch
            {
                EmailType.UserCreated => Resources.EmailTemplates.UserCreatedTemplate,
                _ => throw new ArgumentOutOfRangeException(nameof(mailType), mailType, "Invalid email type."),
            };

            string mailBody = Encoding.UTF8.GetString(templateBytes);

            _logger.LogInformation($"SmtpSettings: {_smtpSettings.FromEmail}");

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Sender Name", _smtpSettings.FromEmail));
            mailMessage.To.Add(new MailboxAddress("Recipient Name", email));
            mailMessage.Subject = "Your MatchMaker-account has been created.";

            var messageBody = new BodyBuilder { HtmlBody = mailBody };
            mailMessage.Body = messageBody.ToMessageBody();

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