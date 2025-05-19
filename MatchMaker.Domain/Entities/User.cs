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
    [BsonElement("passwordHash")]
    public string PasswordHash { get; set; } = null!;

    [BsonElement("email")]
    public string Email { get; set; } = null!;

    [BsonElement("firstName")]
    public string FirstName { get; set; } = null!;

    [BsonElement("lastName")]
    public string LastName { get; set; } = null!;

    [BsonElement("createdDate")]
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    [BsonRepresentation(BsonType.String)]
    [BsonElement("userRole")]
    public UserRole UserRole { get; set; } = UserRole.Unspecified;
    
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("teamAffiliation")]
    public string? TeamAffiliation { get; set; }

    [BsonElement("isVerified")]
    public bool IsVerified { get; set; } = false;
}