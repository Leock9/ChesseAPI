using MongoDB.Driver;

namespace Infrastructure.MongoDb;

public class Context
{
    private readonly IMongoDatabase _database;

    public Context(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<TDocument> GetCollection<TDocument>(string name)
    {
        return _database.GetCollection<TDocument>(name);
    }
}
