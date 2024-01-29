using Domain.Base;
using Domain.Services;
using System.Net;

namespace Api.Endpoints.Order.GetList;

public sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    public ILogger<Endpoint> Log { get; set; } = null!;
    public IOrderService? OrderService { get; set; }

    public override void Configure()
    {
        AllowAnonymous();
        Get("/order/getlist");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        try
        {
            var orders = await OrderService?.GetAll()!;
            await SendAsync(new Response { Orders = orders }, cancellation: c);
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