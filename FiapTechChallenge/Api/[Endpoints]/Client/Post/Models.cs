using FluentValidation;
using System.Net;

namespace Api.Endpoints.Client.Post;

public sealed class Request
{
    public string Name { get; init; } = string.Empty;

    public string Document { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;
}

public sealed class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(x => x.Name)
                            .NotEmpty()
                            .NotNull();

        RuleFor(x => x.Document)
                            .NotEmpty()
                            .NotNull();

        RuleFor(x => x.Email)
                            .NotEmpty()
                            .NotNull();
    }
}

public sealed class Response
{
    public HttpStatusCode StatusCode { get; init; } = HttpStatusCode.Created;
}
