namespace MatchMaker.Domain.DTOs;

public class UserDTO
{
    public string? UserId { get; set; }
    public string? Password { get; set; }
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

public class UserResultDTO
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public UserDTO? userDTO{ get; set; }
}