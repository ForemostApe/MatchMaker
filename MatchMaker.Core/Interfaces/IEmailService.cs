using MimeKit;
using static MatchMaker.Core.Services.EmailService;

namespace MatchMaker.Core.Interfaces;

public interface IEmailService
{
    Task CreateEmailAsync(string email, EmailType mailType, string? token = null);
    Task SendEmailAsync(MimeMessage emailMessage);
}