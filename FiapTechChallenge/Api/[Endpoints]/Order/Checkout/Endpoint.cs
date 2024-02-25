using Domain.Base;
using Domain.Services;
using System.Net;

namespace Api.Endpoints.Checkout.Post;

public sealed class Endpoint : Endpoint<Request, Response, Mapper>
{
    public ILogger<Endpoint> Log { get; set; } = null!;
    public IOrderService? OrderService { get; set; }

    public override void Configure()
    {
        AllowAnonymous();
        Post("/order/checkout");
    }

    public override async Task HandleAsync(Request r, CancellationToken c)
    {
        try
        {
            var orderId = OrderService?.CreateOrder(r.BaseOrderRequest);
            await SendAsync(new Response { OrderId = orderId.GetValueOrDefault()}, (int)HttpStatusCode.Created, cancellation: c);
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