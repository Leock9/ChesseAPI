using Domain;
using Domain.Ports;

namespace Infrastructure.MongoDb.Repository;

public class ClientRepository : IClientRepository
{
    public void Create(Client client)
    {
        throw new NotImplementedException();
    }

    public Task<Client> GetByDocumentAsync(string document)
    {
        throw new NotImplementedException();
    }
}
