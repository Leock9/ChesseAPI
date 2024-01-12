using Domain.Ports;
using Domain.Services.Requests;
using Serilog;

namespace Domain.Services;

public class ClientService(IClientRepositoy clientRepositoy, ILogger logger) : IClientService
{
    public void Create(CreateClientRequest createClientRequest)
    {
        try
        {
            clientRepositoy.Create
                (new Client
                    (
                    createClientRequest.Name, 
                    createClientRequest.Document,
                    createClientRequest.Email
                    ));
        }
        catch(Exception ex)
        {
            logger.Error(ex, ex.Message);
            throw;
        }
    }

    public async Task<Client> GetByDocumentAsync(string document)
    {
        try
        {
            return await clientRepositoy.GetByDocumentAsync(document);
        }
        catch(Exception ex)
        {
            logger.Error(ex, ex.Message);
            throw;
        }
    }
}