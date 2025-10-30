namespace MatchMaker.Domain.DTOs.Users;

public class UserDto
{
    public string Id { get; set; } = null!;
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? TeamAffiliation { get; set; }
    public string? UserRole { get; set; }
}