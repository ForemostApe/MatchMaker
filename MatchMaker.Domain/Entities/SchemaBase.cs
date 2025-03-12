using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MatchMaker.Domain.Entities;

public abstract class SchemaBase<T>
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string ID { get; set; } = null!;
}
