namespace MatchMaker.Domain.DTOs.Users;
public class UpdateUserDTO
{
    public string Id { get; set; } = null!;
    public string? PasswordHash { get; set; }
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserRole { get; set; }
}
