using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Runtime.Serialization;

namespace MatchMaker.Domain.Entities;
public enum GameStatus
{
    Cancelled,
    Draft,
    Planned,
    Signed,
    Booked,
    Completed,  
}

public enum GameType
{
    [EnumMember(Value = "7v7")]
    SevenVSeven,

    [EnumMember(Value = "9v9")]
    NineVNine,

    [EnumMember(Value = "11v11")]
    ElevenVEleven
}

public class Game : SchemaBase<Game>
{
    public DateTime StartTime { get; set; }
    public string GameType { get; set; } = null!;
    public string Location { get; set; } = null!;
    public GameStatus GameStatus { get; set; } = GameStatus.Draft;

    [BsonRepresentation(BsonType.ObjectId)]
    public string HomeTeamId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string AwayTeamId { get; set; } = null!;

    [BsonRepresentation(BsonType.ObjectId)]
    public string RefereeId { get; set; } = null!;
    public bool IsCoachSigned { get; set; } = false;
    public DateTime? CoachSignedDate { get; set; }
    public bool IsRefereeSigned { get; set; } = false;
    public DateTime? RefereeSignedDate { get; set; }

    public Conditions Conditions { get; set; } = null!;
}

public class Conditions
{
    public string Court { get; set; } = null!;
    public string Timing { get; set; } = null!;  
    public string OffensiveConditions { get; set; } = null!;
    public string DefensiveConditions { get; set; } = null!;
    public string Specialists { get; set; } = null!;
    public string Penalties { get; set; } = null!;
}