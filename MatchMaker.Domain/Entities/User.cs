using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MatchMaker.Domain.Entities;

public enum UserRole
{
    Admin,
    Coach,
    Referee,
    Functionary,
    Guest,
    Unspecified
};

public class User : SchemaBase<User>
{
    public string PasswordHash { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [BsonRepresentation(BsonType.String)]
    public UserRole UserRole { get; set; } = UserRole.Unspecified;
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string? TeamAffiliation { get; set; }
    public bool IsVerified { get; set; } = false;
}