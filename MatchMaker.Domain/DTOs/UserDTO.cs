namespace MatchMaker.Domain.DTOs;

public class UserDTO
{
    public string? UserID { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Role { get; set; }
}

public class CreateUserDTO
{
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;  
}