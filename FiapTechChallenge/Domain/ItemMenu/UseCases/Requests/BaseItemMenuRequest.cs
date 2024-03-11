using Domain.ValueObjects;

namespace Domain.Services.Requests;

public record BaseItemMenuRequest
(
        string Name,
        string Description,
        decimal Price,
        int Stock,
        IEnumerable<Ingredient> Ingredients,
        Size Size,
        Category Category
);
