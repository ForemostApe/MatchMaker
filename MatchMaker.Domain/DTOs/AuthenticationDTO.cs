namespace MatchMaker.Domain.DTOs;

public class AuthenticationDTO
{
    public string AccessToken { get; set; } = null!;
    public AuthenticatedUserDTO User { get; set; } = null!;
}
