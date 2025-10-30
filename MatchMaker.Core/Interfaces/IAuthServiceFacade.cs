using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Authentication;

namespace MatchMaker.Core.Interfaces;
public interface IAuthServiceFacade
{
    Task<Result<bool>> VerifyEmailAsync(string token);
    Task<Result<AuthenticationDto>> LoginAsync(LoginDto loginDto);
    Result<string> Logout();
    Task<Result<AuthenticationDto>> GenerateRefreshTokenAsync(string refreshToken);
}
