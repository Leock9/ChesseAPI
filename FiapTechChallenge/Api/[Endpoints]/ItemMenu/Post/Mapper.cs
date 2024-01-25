using Domain.Services.Requests;

namespace Api.Endpoints.ItemMenu.Post;

public sealed class Mapper : Mapper<Request, Response, object>
{
    public BaseItemMenuRequest ToRequest(Request r) => new
        (
           r.Name,
           r.Description,
           r.Price,
           r.Stock,
           r.Ingredients,
           r.Size,
           Domain.ItemMenu.GetCategory(r.CategoryId)
         );
}