namespace Api.Endpoints.Client
{
    public sealed class Mapper : Mapper<Request, Response, object>
    {
        public Response ToResponse(Domain.Client c) => new()
        {
            Id = c.Id,
            Name = c.Name,
            Document = c.Document,
            Email = c.Email
        };
    }
}