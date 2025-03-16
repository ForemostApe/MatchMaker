namespace MatchMaker.Core.Interfaces;

public interface IEmailTemplateService
{
    Task<string> GetEmailTemplateAsync(string templateName);
}
