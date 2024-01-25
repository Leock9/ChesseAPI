using Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.MongoDb.Documents;

public record ItemMenuDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; init; }

    [BsonElement("name")]
    public string Name { get; init; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; init; } = string.Empty;

    [BsonElement("price")]
    public decimal Price { get; init; }

    [BsonElement("stock")]
    public int Stock { get; init; }

    [BsonElement("ingredients")]
    public IEnumerable<Ingredient> Ingredients { get; init; } = new List<Ingredient>();

    [BsonElement("size")]
    public Size Size { get; init; }

    [BsonElement("additionals")]
    public IEnumerable<Additional> Additionals { get; init; } = new List<Additional>();

    [BsonElement("category")]
    public Category Category { get; init; }

    [BsonElement("isactive")]
    public bool IsActive { get; init; } = false;

    [BsonElement("createat")]
    public DateTime CreateAt { get; init; }

    [BsonElement("updateat")]
    public DateTime UpdateAt { get; init; }
}
