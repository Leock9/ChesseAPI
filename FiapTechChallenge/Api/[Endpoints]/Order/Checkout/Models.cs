using Domain.Services.Requests;
using FluentValidation;

namespace Api.Endpoints.Checkout.Post;

public sealed class Request
{
    public BaseOrderRequest BaseOrderRequest { get; set; } = null!;
}

public sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.BaseOrderRequest.ItemMenuIds).NotEmpty().NotNull();
        RuleFor(x => x.BaseOrderRequest.Document).NotEmpty().NotNull();  
        RuleFor(x => x.BaseOrderRequest.TotalOrder).NotEmpty().NotNull();
    }
}

public sealed class Response
{
    public Guid OrderId { get; init; }
}
