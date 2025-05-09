using MatchMaker.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Utilities
{
    public class SessionManager(ILogger<SessionManager> logger, IHttpContextAccessor httpContextAccessor, ICookieFactory cookieFactory) : ISessionManager
    {
        private readonly ILogger<SessionManager> _logger = logger;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ICookieFactory _cookieFactory = cookieFactory;

        public void ClearSession(string tokenName)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;

                if (httpContext == null)
                {
                    _logger.LogError("HTTP Context was null.");
                    throw new InvalidOperationException("HTTP context is not available.");
                }

                httpContext.Session.Clear();
                httpContext.Response.Cookies.Delete(tokenName);

                _cookieFactory.ExpireCookie(tokenName);
            }
            catch
            {
                throw;
            }
        }
    }
}
