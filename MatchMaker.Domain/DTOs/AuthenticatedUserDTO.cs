namespace MatchMaker.Domain.DTOs;

public class AuthenticatedUserDTO
{
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string UserRole { get; set; } = null!;
}
