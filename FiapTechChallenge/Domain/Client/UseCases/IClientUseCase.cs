using Domain.Services.Requests;

namespace Domain.Services;

public interface IClientUseCase
{
    void Create(CreateClientRequest createClientRequest);

    Task<Client> GetByDocumentAsync(string document);
}