using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessToken(User user);
    Task<string> GenerateRefreshToken(User user);
    Task<User> ValidateRefreshToken(string refreshToken);
}
