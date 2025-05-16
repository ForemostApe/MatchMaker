using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MatchMaker.Domain.DTOs.Games;
public class UpdateGameDTO
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public DateTime? StartTime { get; set; }
    public string? Location { get; set; }
    public string? GameType { get; set; }

    [BsonRepresentation(BsonType.ObjectId)]
    public string? RefereeId { get; set; }
    public UpdateConditionsDTO? Conditions { get; set; }
}

public class UpdateConditionsDTO
{
    public string? Court { get; set; }
    public string? Timing { get; set; }
    public string? OffensiveConditions { get; set; }
    public string? DefensiveConditions { get; set; }
    public string? Specialists { get; set; }
    public string? Penalties { get; set; }
}