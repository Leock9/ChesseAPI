namespace Api.Endpoints.Order.GetList;

public sealed class Request
{
}

public sealed class Validator : Validator<Request>
{
    public Validator()
    {
    }
}

public sealed class Response
{
    public IEnumerable<Domain.Order> Orders { get; init; } = new List<Domain.Order>();
}
