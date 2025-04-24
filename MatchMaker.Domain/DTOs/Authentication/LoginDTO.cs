namespace MatchMaker.Domain.DTOs.Authentication;

public class LoginDTO
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
