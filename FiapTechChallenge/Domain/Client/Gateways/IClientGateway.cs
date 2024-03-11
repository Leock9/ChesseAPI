namespace Domain.Ports;

public interface IClientGateway
{
    void Create(Client client);

    Task<Client> GetByDocumentAsync(string document);
}
