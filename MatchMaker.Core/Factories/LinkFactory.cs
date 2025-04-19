using MatchMaker.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Factories;

public class LinkFactory(ILogger<LinkFactory> logger, string clientUrl) : ILinkFactory
{
    private readonly ILogger _logger = logger;
    private readonly string _clientUrl = !string.IsNullOrWhiteSpace(clientUrl) ? clientUrl :  throw new ArgumentNullException(nameof(clientUrl));

    public string CreateVerificationLink(string token)
    {
        _logger.LogInformation("Creating verification-link for mail.");
        return $"{_clientUrl}/api/Auth/verify-email?verificationToken={token}";
    }
    public string CreateResetPasswordLink(string email)
    {
        throw new NotImplementedException();
    }
}
