using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MatchMaker.Domain.Entities;
public enum GameStatus
{
    Cancelled,
    Draft,
    Planned,
    Booked,
    Completed,  
}

public class Game : SchemaBase<Game>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string GameType { get; set; } = null!;
    public string Location { get; set; } = null!;
    public GameStatus GameStatus { get; set; } = GameStatus.Draft;

    [BsonRepresentation(BsonType.ObjectId)]
    public string HomeTeamId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string AwayTeamId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string RefereeId { get; set; } = null!;
    public Conditions Conditions { get; set; } = null!;
}

public class Conditions
{
    public string Court { get; set; }
    public string OffensiveConditions { get; set; }
    public string DefensiveConditions { get; set; }
    public string Specialists { get; set; }
    public string Penalties { get; set; }
}