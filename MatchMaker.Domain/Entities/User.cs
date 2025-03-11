using MongoDB.Entities;

namespace MatchMaker.Domain.Entities;
public class User : Entity
{
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = null!;
}