using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace MatchMaker.Domain.Entities;

public enum GameStatus
{
    Cancelled,
    Draft,
    Planned,
    Signed,
    Booked,
    Completed
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
    [BsonElement("startTime")]
    public DateTime StartTime { get; set; }

    [BsonElement("gameType")]
    [BsonRepresentation(BsonType.String)]
    public string GameType { get; set; } = null!;

    [BsonElement("location")]
    public string Location { get; set; } = null!;

    [BsonElement("gameStatus")]
    [BsonRepresentation(BsonType.String)]
    public GameStatus GameStatus { get; set; } = GameStatus.Draft;

    [BsonElement("homeTeamId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string HomeTeamId { get; set; } = null!;

    [BsonElement("awayTeamId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string AwayTeamId { get; set; } = null!;

    [BsonElement("refereeId")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string RefereeId { get; set; } = null!;

    [BsonElement("isCoachSigned")]
    public bool IsCoachSigned { get; set; } = false;

    [BsonElement("coachSignedDate")]
    public DateTime? CoachSignedDate { get; set; }

    [BsonElement("isRefereeSigned")]
    public bool IsRefereeSigned { get; set; } = false;

    [BsonElement("refereeSignedDate")]
    public DateTime? RefereeSignedDate { get; set; }

    [BsonElement("conditions")]
    public Conditions Conditions { get; set; } = new();
}

public class Conditions

{
    [BsonElement("court")]
    [Required, MinLength(1)]
    public string Court { get; set; } = null!;

    [BsonElement("timing")]
    [Required, MinLength(1)]
    public string Timing { get; set; } = null!;

    [BsonElement("offensiveConditions")]
    [Required, MinLength(1)]
    public string OffensiveConditions { get; set; } = null!;

    [BsonElement("defensiveConditions")]
    [Required, MinLength(1)]
    public string DefensiveConditions { get; set; } = null!;

    [BsonElement("specialists")]
    [Required, MinLength(1)]
    public string Specialists { get; set; } = null!;

    [BsonElement("penalties")]
    [Required, MinLength(1)]
    public string Penalties { get; set; } = null!;
}
