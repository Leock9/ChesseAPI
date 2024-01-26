using Domain;
using Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.MongoDb.Documents;

public record OrderDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; init; }

    [BsonElement("totalorder")]
    public decimal TotalOrder { get; init; }

    [BsonElement("status")]
    public Status Status { get; init; }

    [BsonElement("document")]
    public string Document { get; init; }

    [BsonElement("itemmenus")]
    public IList<ItemMenu> ItemMenus { get; init; }

    [BsonElement("createdat")]
    public DateTime CreatedAt { get; init; }

    [BsonElement("updatedat")]
    public DateTime UpdatedAt { get; init; } 
}
