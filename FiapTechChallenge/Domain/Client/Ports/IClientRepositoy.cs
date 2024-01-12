namespace Domain.Ports;

public interface IClientRepositoy
{
    void Create(Client client);

    Task<Client> GetByDocumentAsync(string document);
}
