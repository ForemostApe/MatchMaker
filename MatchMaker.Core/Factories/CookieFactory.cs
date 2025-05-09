using MatchMaker.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace MatchMaker.Core.Factories;

public class CookieFactory(IHttpContextAccessor httpContextAccessor) : ICookieFactory
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public void CreateHttpOnlyCookie(string tokenName, string token)
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

    public void DeleteCookie(string tokenName)
    {
        var response = _httpContextAccessor.HttpContext?.Response ?? throw new InvalidOperationException("HTTP response is not available.");

        response.Cookies.Delete(tokenName);

        var cookieOptions = new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(-1)
        };

        response.Cookies.Append(tokenName, string.Empty, cookieOptions);
    }
}