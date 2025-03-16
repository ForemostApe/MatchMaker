using MimeKit;

namespace MatchMaker.Core.Interfaces;

public interface IEmailService
{
    Task CreateMailContentAsync(string userEmailAddress, string mailType);
    Task SendEmailAsync(MimeMessage emailMessage);
}