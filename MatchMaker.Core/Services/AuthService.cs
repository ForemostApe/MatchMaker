using MatchMaker.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services
{
    public class AuthService(ILogger<AuthService> logger) : IAuthService
    {
        private readonly ILogger<AuthService> _logger = logger;

        public string HashPassword(string password)
        {
            _logger.LogInformation("Trying to hash and salt password");
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
