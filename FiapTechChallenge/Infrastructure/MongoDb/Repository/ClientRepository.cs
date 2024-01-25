using Domain;
using Domain.Ports;
using Infrastructure.MongoDb.Documents;
using MongoDB.Driver;

namespace Infrastructure.MongoDb.Repository;

public class ClientRepository : IClientRepository
{
    private readonly Context _context;

    public ClientRepository(Context context)
    {
        _context = context;
    }

    public void Create(Client client)
    {
        var document = new ClientDocument
        {
            Id = client.Id,
            Document = client.Document,
            Email = client.Document,
            Name = client.Name
        };
        _context.GetCollection<ClientDocument>("clients").InsertOne(document);
    }

    public Task<Client> GetByDocumentAsync(string document)
    {
        var clientDocument = _context.GetCollection<ClientDocument>("clients")
                                          .Find(x => x.Document == document)
                                          .FirstOrDefaultAsync();

        return clientDocument.ContinueWith
            (x => new Client
             (
                x.Result.Name,
                x.Result.Document,
                x.Result.Email
             )
            );
    }
}
