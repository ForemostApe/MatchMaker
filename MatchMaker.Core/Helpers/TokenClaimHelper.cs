using MatchMaker.Domain.Entities;
using System.Security.Claims;

namespace MatchMaker.Core.Helpers
{
    public static class TokenClaimHelper
    {
        public static TokenClaims CheckAndExtractClaims(ClaimsPrincipal principal)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new UnauthorizedAccessException($"User-Id claim saknas.");

            var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value
                ?? throw new UnauthorizedAccessException($"User-email claim saknas.");

            var tokenUsage = principal.FindFirst("token_usage")?.Value
                ?? throw new UnauthorizedAccessException($"Token-claim is saknas.");

            return new TokenClaims
            {
                UserId = userId,
                UserEmail = userEmail,
                TokenUsage = tokenUsage
            };
        }
    }
}
