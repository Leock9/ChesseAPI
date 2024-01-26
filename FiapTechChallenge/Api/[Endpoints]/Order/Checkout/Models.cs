using Domain.Services.Requests;
using FluentValidation;
using System.Net;

namespace Api.Endpoints.Checkout.Post;

public sealed class Request
{
    public BaseOrderRequest BaseOrderRequest { get; set; } = null!;
}

public sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.BaseOrderRequest.ItemMenus).NotEmpty().NotNull();
        RuleFor(x => x.BaseOrderRequest.Document).NotEmpty().NotNull();  
        RuleFor(x => x.BaseOrderRequest.TotalOrder).NotEmpty().NotNull();
    }
}

public sealed class Response
{
    public string StatusCode { get; init; } = HttpStatusCode.Created.ToString();
}
