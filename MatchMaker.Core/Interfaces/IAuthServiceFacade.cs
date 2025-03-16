using MatchMaker.Domain.DTOs;

namespace MatchMaker.Core.Interfaces;
public interface IAuthServiceFacade
{
    Task<Result<AuthenticationDTO>> LoginAsync(LoginDTO loginDTO);
    Result<string> Logout();
    Task<bool> RefreshTokenAsync(string refreshToken);
}
