using MatchMaker.Domain.Enums;
using MimeKit;

namespace MatchMaker.Core.Interfaces;

public interface IEmailService
{
    Task CreateEmailAsync(string email, EmailType mailType, string? token = null);
    Task SendEmailAsync(MimeMessage emailMessage);
}