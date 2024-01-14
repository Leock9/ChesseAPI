using Domain.Services.Requests;

namespace Api.Endpoints.Client.Post;

public sealed class Mapper : Mapper<Request, Response, object>
{
    public CreateClientRequest ToRequest(Request r) => new
        (
            r.Name,
            r.Document,
            r.Email
        );
}