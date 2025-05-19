using MatchMaker.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Factories;

public class LinkFactory(ILogger<LinkFactory> logger, string clientUrl) : ILinkFactory
{
    private readonly ILogger _logger = logger;
    private readonly string _clientUrl = !string.IsNullOrWhiteSpace(clientUrl) ? clientUrl :  throw new ArgumentNullException(nameof(clientUrl));

    public string CreateVerificationLink(string token)
    {
        try
        {
            return $"{_clientUrl}/verify-email?verificationToken={token}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while trying to create a verification-link.");
            throw;
        }
    }
    public string CreateResetPasswordLink(string email)
    {
        throw new NotImplementedException();
    }
}
