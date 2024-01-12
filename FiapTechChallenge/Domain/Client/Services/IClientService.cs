using Domain.Services.Requests;

namespace Domain.Services;

public interface IClientService
{
    void Create(CreateClientRequest createClientRequest);

    Task<Client> GetByDocumentAsync(string document);
}