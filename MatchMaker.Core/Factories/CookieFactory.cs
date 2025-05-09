using MatchMaker.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MatchMaker.Core.Factories;

public class CookieFactory(IHttpContextAccessor httpContextAccessor) : ICookieFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void CreateHttpOnlyCookie(string tokenName, string token)
    {
        try
        {
            var response = _httpContextAccessor.HttpContext?.Response ?? throw new InvalidOperationException("HTTP response is not available.");

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddHours(7)
            };

            response.Cookies.Append(tokenName, token, cookieOptions);
        }
        catch
        {
            throw;
        }
    }

    public void ExpireCookie(string tokenName)
    {
        try
        {
            var response = _httpContextAccessor.HttpContext?.Response ?? throw new InvalidOperationException("HTTP response is not available.");

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            response.Cookies.Append(tokenName, string.Empty, cookieOptions);
        }
        catch
        {
            throw;
        }
    }
}