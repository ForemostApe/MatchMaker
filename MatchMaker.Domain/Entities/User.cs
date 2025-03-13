namespace MatchMaker.Domain.Entities;

public enum UserRole
{
    Admin,
    Coach,
    Functionary,
    Guest,
    Referee
};

public class User : SchemaBase<User>
{
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public UserRole UserRole { get; set; } = UserRole.Guest;
}