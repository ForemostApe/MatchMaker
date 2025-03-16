using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MatchMaker.Core.Services;

public class EmailTemplateService(ILogger<EmailTemplateService> logger, IOptions<EmailSettings> emailSettings) : IEmailTemplateService
{
    private readonly ILogger<EmailTemplateService> _logger = logger;
    private readonly string _templateDirectory = emailSettings.Value.TemplateDirectory ?? throw new ArgumentNullException(nameof(emailSettings));

    public async Task<string> GetEmailTemplateAsync(string templateName)
    {
        string templateFilePath = Path.Combine(_templateDirectory, $"{templateName}Template.html");

        if (!File.Exists(templateFilePath))
        {
            _logger.LogError("Couldn't find the template file at {templateFilePath}.", nameof(templateFilePath));
            throw new FileNotFoundException($"Template-file couldn't be found: {templateFilePath}");
        }

        _logger.LogInformation("Template successfully fetched from {templateFilePath}.", nameof(templateFilePath));
        return await File.ReadAllTextAsync(templateFilePath);
    }
}