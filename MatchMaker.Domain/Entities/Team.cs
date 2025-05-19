using MongoDB.Bson.Serialization.Attributes;

namespace MatchMaker.Domain.Entities;
public class Team : SchemaBase<Team>
{
    [BsonElement("teamName")]
    public string TeamName { get; set; } = null!;

    [BsonElement("teamLogo")]
    public string TeamLogo { get; set; } = null!;
}