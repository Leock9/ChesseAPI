using Domain.Base;
using Domain.Services;
using System.Net;

namespace Api.Endpoints.Client;

public sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    public ILogger<Endpoint> Log { get; set; } = null!;
    public IClientService? ClientService { get; }

    public override void Configure()
    {
        Get("/client/GetByDocument");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        try
        {
            var result = await ClientService!.GetByDocumentAsync(r.Document);

            if (result is null) ThrowError("Client not found", (int)HttpStatusCode.NotFound);

            await SendAsync(Map.ToResponse(result), cancellation: c);
        }
        catch (DomainException dx)
        {
            ThrowError(dx.Message);
        }
        catch (Exception ex)
        {
            Log.LogError("Ocorreu um erro inesperado ao executar o endpoint:{typeof(Endpoint).Namespace}. {ex.Message}", typeof(Endpoint).Namespace, ex.Message);
            ThrowError("Unexpected Error", (int)HttpStatusCode.BadRequest);
        }
    }
}