using MailKit.Net.Smtp;
using MailKit.Security;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Reflection;

namespace MatchMaker.Core.Services;

public class EmailService(ILogger<EmailService> logger, IOptions<SmtpSettings> smtpSettings, IOptions<EmailSettings> emailSettings) : IEmailService
{
    private readonly ILogger<EmailService> _logger = logger;
    private readonly SmtpSettings _smtpSettings = smtpSettings.Value;
    private readonly EmailSettings _emailSettings = emailSettings.Value;

    public enum EmailType
    {
        UserCreated,
        PasswordReset,
        LoginLink,
    }

    public async Task CreateEmailAsync(string email, EmailType mailType)
    {
        {
            string templatePath = mailType switch
            {
                EmailType.UserCreated => GetTemplatePath("UserCreatedTemplate.html"),
                _ => throw new ArgumentOutOfRangeException(nameof(mailType), mailType, "Invalid email type."),
            };

            string mailBody = await LoadEmailTemplateAsync(templatePath);

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Sender Name", _smtpSettings.SmtpFromEmail));
            mailMessage.To.Add(new MailboxAddress("Recipient Name", email));
            mailMessage.Subject = "Your MatchMaker-account has been created.";

            var messageBody = new BodyBuilder { HtmlBody = mailBody };
            mailMessage.Body = messageBody.ToMessageBody();

            _logger.LogInformation("UserCreated email content successfully created.");
            await SendEmailAsync(mailMessage);
        }
    }

    public async Task<string> LoadEmailTemplateAsync(string templatePath)
    {
        if (!File.Exists(templatePath))
        {
            throw new FileNotFoundException($"Template file not found: {templatePath}");
        }

        return await File.ReadAllTextAsync(templatePath);
    }

    public string GetTemplatePath(string templateFileName)
    {
        string currentDirectory = Directory.GetCurrentDirectory();
        string combinedPath = Path.Combine(currentDirectory, _emailSettings.TemplateDirectory);
        string emailTemplatesPath = Path.GetFullPath(combinedPath);

        _logger.LogInformation("Resolved email templates path: {EmailTemplatesPath}", emailTemplatesPath);

        return Path.Combine(emailTemplatesPath, templateFileName);
    }

    public async Task SendEmailAsync(MimeMessage mailMessage)
    {
        using var smtpClient = new SmtpClient();

        try
        {
            _logger.LogInformation("Trying to connecting to SMTP server {SmtpHost}:{SmtpPort}", _smtpSettings.SmtpHost, _smtpSettings.SmtpPort);

            await smtpClient.ConnectAsync(_smtpSettings.SmtpHost, _smtpSettings.SmtpPort, SecureSocketOptions.StartTls);
            _logger.LogInformation("Initiated STARTTLS.");

            await smtpClient.AuthenticateAsync(_smtpSettings.SmtpUsername, _smtpSettings.SmtpPassword);
            _logger.LogInformation("Authenticated by SMTP-server.");

            _logger.LogInformation("Sending email to {userEmailAddress}.", mailMessage.To);
            await smtpClient.SendAsync(mailMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while trying to send email to {userEmailAddress}.", mailMessage.To);
            throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
        }
        finally
        {
            await smtpClient.DisconnectAsync(true);
        }
    }
}