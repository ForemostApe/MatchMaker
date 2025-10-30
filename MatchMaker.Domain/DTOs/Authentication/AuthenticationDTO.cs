using MatchMaker.Domain.DTOs.Users;

namespace MatchMaker.Domain.DTOs.Authentication;

public class AuthenticationDto
{
    public string AccessToken { get; set; } = null!;
    public AuthenticatedUserDto User { get; set; } = null!;
}
