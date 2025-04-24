using MatchMaker.Domain.DTOs.Users;

namespace MatchMaker.Domain.DTOs.Authentication;

public class AuthenticationDTO
{
    public string AccessToken { get; set; } = null!;
    public AuthenticatedUserDTO User { get; set; } = null!;
}
