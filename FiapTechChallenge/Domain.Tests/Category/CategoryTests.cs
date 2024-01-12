using Bogus;
using Domain.Base;
using FluentAssertions;

namespace Domain.Tests;

public class CategoryTests
{
    [Fact]
    public void CreateCategory()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.Categories(1).FirstOrDefault();

        var category = new Category(name!);

        category.Should()
                .Match<Category>(c => c.Name == name)
                .And.NotBeNull();
    }

    [Fact]
    public void CreateCategoryWhenNameIsEmpty()
    {
        var name = string.Empty;

        Action action = () =>
        {
            new Category(name);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Name is required");
    }

    [Fact]
    public void UpdateCategory()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.Categories(1).FirstOrDefault();
        var category = new Category(name!);
        var newName = faker.Commerce.Categories(1).FirstOrDefault();

        var updatedCategory = category.Update(new Category(newName!));

        updatedCategory.Should()
                       .Match<Category>(c => c.Name == newName)
                       .And.NotBeNull();
    }
}
