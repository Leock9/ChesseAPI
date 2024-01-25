using FluentValidation;
using System.Net;

namespace Api.Endpoints.ItemMenu.GetCategory;

public sealed class Request
{
    public int CategoryId { get; init; }
}

public sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.CategoryId)
                            .LessThan (0)
                            .GreaterThan(5)
                            .NotEmpty()
                            .NotNull();
                           
    }
}

public sealed class Response
{
    public string StatusCode { get; init; } = HttpStatusCode.OK.ToString();
}
