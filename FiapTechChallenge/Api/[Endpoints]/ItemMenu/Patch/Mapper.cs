using Domain.Services.Requests;

namespace Api.Endpoints.ItemMenu.Patch;

public sealed class Mapper : Mapper<Request, Response, object>
{
    public UpdateItemMenuRequest ToRequest(Request r) => new
    (
       r.Id,
       r.Name,
       r.Description,
       r.Price.GetValueOrDefault(),
       r.Stock,
       r.Ingredients!,
       r.Size.GetValueOrDefault(),
       Domain.ItemMenu.GetCategory(r.CategoryId.GetValueOrDefault())
    );
}