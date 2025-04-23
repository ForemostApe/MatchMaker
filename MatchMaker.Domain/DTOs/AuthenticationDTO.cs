namespace MatchMaker.Domain.DTOs;

public class AuthenticationDTO
{
    public string AccessToken { get; set; } = null!;
    public UserDTO User { get; set; } = null!;
}
