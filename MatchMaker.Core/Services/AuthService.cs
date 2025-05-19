using MatchMaker.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services
{
    public class AuthService(ILogger<AuthService> logger) : IAuthService
    {
        private readonly ILogger<AuthService> _logger = logger;

        public string HashPassword(string password)
        {
            ArgumentException.ThrowIfNullOrEmpty(password);

            try
            {
                return BCrypt.Net.BCrypt.HashPassword(password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occured while trying to hash password.");
                throw;
            }
        }
    }
}
