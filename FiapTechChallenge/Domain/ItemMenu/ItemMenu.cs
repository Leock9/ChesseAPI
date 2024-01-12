using Domain.Base;
using Domain.ValueObjects;

namespace Domain;

public record ItemMenu
(
    string Name, 
    string Description, 
    decimal Price, 
    int Stock, 
    IEnumerable<Ingredient> Ingredients,
    Size Size
)
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; init; } = string.IsNullOrEmpty(Name) ?
                                        throw new DomainException("Name is required") : Name;

    public string Description { get; init; } = string.IsNullOrEmpty(Description) ?
                                               throw new DomainException("Description is required") : Description;

    public decimal Price { get; init; } = Price <= 0 ?
                                          throw new DomainException("Price is required") : Price;

    public int Stock { get; init; } = Stock <= 0 ?
                                      throw new DomainException("Stock is required") : Stock;

    public IEnumerable<Ingredient> Ingredients { get; init; } = Ingredients.Any() ? Ingredients :
                                            throw new DomainException("Ingredients is required");

    public Size Size { get; init; } =  Size;

    public IEnumerable<Additional> Additionals { get; init; } = new List<Additional>();

    public bool IsActive { get; set; } = false;

    public void Activate()
    {
       if(IsAvailable()) IsActive = true;
       else throw new DomainException("ItemMenu is invalid to be activated!");
    }

    public void Deactivate() => IsActive = false;

    public ItemMenu Update(ItemMenu itemMenu)
    {
        var updatedItemMenu = this with
        {
            Id = this.Id,
            Name = itemMenu.Name ?? this.Name,
            Description = itemMenu.Description ?? this.Description,
            Price = itemMenu.Price,
            Stock = itemMenu.Stock,
            Ingredients = itemMenu.Ingredients ?? this.Ingredients,
            Size = itemMenu.Size,
            Additionals = itemMenu.Additionals ?? this.Additionals,
        };

        if (updatedItemMenu.Stock < 0)
            updatedItemMenu = updatedItemMenu with { IsActive = false };

        return updatedItemMenu;
    }

    public bool IsAvailable() => Stock > 0 && Price > 0 && Ingredients.Any();
}