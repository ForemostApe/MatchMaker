namespace MatchMaker.Domain.DTOs.Users;

public class AuthenticatedUserDTO
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? TeamAffiliation { get; set; }
    public string? UserRole { get; set; }
}
