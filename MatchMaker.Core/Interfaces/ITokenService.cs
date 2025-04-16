using MatchMaker.Domain.Entities;
using System.Security.Claims;

namespace MatchMaker.Core.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessToken(User user);
    Task<string> GenerateRefreshToken(User user);
    Task<User> ValidateRefreshToken(string refreshToken);
    ClaimsPrincipal DecryptToken(string encryptedToken);
}
