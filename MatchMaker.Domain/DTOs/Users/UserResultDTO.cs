namespace MatchMaker.Domain.DTOs.Users;

public class UserResultDTO
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public UserDTO? UserDTO { get; set; }
}
