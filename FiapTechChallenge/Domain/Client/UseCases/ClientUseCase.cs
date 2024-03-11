using Domain.Ports;
using Domain.Services.Requests;
using Microsoft.Extensions.Logging;

namespace Domain.Services;

public class ClientUseCase: IClientUseCase
{
    private readonly ILogger<ClientUseCase> _logger;
    private readonly IClientGateway _clientRepositoy;

    public ClientUseCase
    (
        ILogger<ClientUseCase> logger,
        IClientGateway clientRepositoy
    )
    {
        _logger = logger;
        _clientRepositoy = clientRepositoy;
    }

    public void Create(CreateClientRequest createClientRequest)
    {
        try
        {
            _clientRepositoy.Create
                (new Client
                    (
                    createClientRequest.Name, 
                    createClientRequest.Document,
                    createClientRequest.Email
                    ));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<Client> GetByDocumentAsync(string document)
    {
        try
        {
            return await _clientRepositoy.GetByDocumentAsync(document);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}