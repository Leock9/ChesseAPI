namespace Domain.Ports;

public interface IClientRepository
{
    void Create(Client client);

    Task<Client> GetByDocumentAsync(string document);
}
