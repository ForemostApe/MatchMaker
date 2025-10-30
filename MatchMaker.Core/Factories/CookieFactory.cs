using MatchMaker.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Factories;

public class CookieFactory(ILogger<CookieFactory> logger, IHttpContextAccessor httpContextAccessor) : ICookieFactory
{
    private readonly ILogger<CookieFactory> _logger = logger;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void CreateHttpOnlyCookie(string tokenName, string token)
    {
        try
        {
            var response = _httpContextAccessor.HttpContext?.Response 
                           ?? throw new InvalidOperationException("HTTP response is not available.");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(7)
            };

            response.Cookies.Append(tokenName, token, cookieOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, 
                "An unexpected error occurred in CookieFactory while trying to create HTTP-only cookie"
            );
            throw;
        }
    }

    public void ExpireCookie(string tokenName)
    {
        try
        {
            var response = _httpContextAccessor.HttpContext?.Response 
                           ?? throw new InvalidOperationException("HTTP response is not available.");

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            response.Cookies.Append(tokenName, string.Empty, cookieOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in CookieFactory while trying to expire cookie");
            throw;
        }
    }
}