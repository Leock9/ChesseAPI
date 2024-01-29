using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Infrastructure.MongoDb.Documents;

public record ClientDocument
{
    [BsonId]
    public Guid Id { get; init; }

    [BsonElement("name")]
    public string Name { get; init; }

    [BsonElement("document")]
    public string Document { get; init; }

    [BsonElement("email")]
    public string Email { get; init; } 
}
