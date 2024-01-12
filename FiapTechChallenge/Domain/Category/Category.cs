using Domain.Base;

namespace Domain;

public record Category(string Name)
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; init; } = string.IsNullOrEmpty(Name) ?
                                        throw new DomainException("Name is required") : Name;

    public bool IsActive { get; set; } = false;

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    public Category Update(Category category)
    {
        var updatedCategory = this with
        {
            Name = category.Name ?? this.Name,
        };

        return updatedCategory;
    }
}
