using Domain.Base;
using Domain.Services;
using System.Net;

namespace Api.Endpoints.Client.Post;

public sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    public ILogger<Endpoint> Log { get; set; } = null!;
    public IClientService? ClientService { get; }

    public override void Configure()
    {
        Post("/client");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        try
        {
            ClientService?.Create(Map.ToRequest(r));
            await SendAsync(new Response(), cancellation: c);
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