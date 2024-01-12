﻿using Bogus;
using Domain.Base;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests;

public class ItemMenuTests
{
    [Fact]
    public void CreateItemMenu()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 100);
        var stock = faker.Random.Int(1, 100);
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        var itemMenu = new ItemMenu(name, description, price, stock, ingredients, size);

        itemMenu.Should()
                .Match<ItemMenu>(i => i.Name == name)
            .And.Match<ItemMenu>(i => i.Description == description)
            .And.Match<ItemMenu>(i => i.Price == price)
            .And.Match<ItemMenu>(i => i.Stock == stock)
            .And.Match<ItemMenu>(i => i.Ingredients == ingredients)
            .And.Match<ItemMenu>(i => i.Size == size)
            .And.NotBeNull();
    }

    [Fact]
    public void CreateItemMenuWhenNameIsEmpty()
    {
        var faker = new Faker("pt_BR");
        var name = string.Empty;
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 100);
        var stock = faker.Random.Int(1, 100);
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        Action action = () =>
        {
            new ItemMenu(name, description, price, stock, ingredients, size);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Name is required");
    }

    [Fact]
    public void CreateItemMenuWhenDescriptionIsEmpty()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = string.Empty;
        var price = faker.Random.Decimal(1, 100);
        var stock = faker.Random.Int(1, 100);
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        Action action = () =>
        {
            new ItemMenu(name, description, price, stock, ingredients, size);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Description is required");
    }

    [Fact]
    public void CreateItemMenuWhenPriceIsLessOrEqualThanZero()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = 0m;
        var stock = faker.Random.Int(1, 100);
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        Action action = () =>
        {
            new ItemMenu(name, description, price, stock, ingredients, size);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Price is required");
    }

    [Fact]
    public void CreateItemMenuWhenStockIsLessOrEqualThanZero()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 100);
        var stock = 0;
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        Action action = () =>
        {
            new ItemMenu(name, description, price, stock, ingredients, size);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Stock is required");
    }

    [Fact]
    public void CreateItemMenuWhenIngredientsIsEmpty()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 100);
        var stock = faker.Random.Int(1, 100);
        var ingredients = new List<Ingredient>();
        var size = Size.M;

        Action action = () =>
        {
            new ItemMenu(name, description, price, stock, ingredients, size);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Ingredients is required");
    }

    [Fact]
    public void ActivateItemMenu()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 100);
        var stock = faker.Random.Int(1, 100);
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        var itemMenu = new ItemMenu(name, description, price, stock, ingredients, size);

        itemMenu.Activate();

        itemMenu.Should()
                .Match<ItemMenu>(i => i.IsActive == true)
            .And.NotBeNull();
    }

    [Fact]
    public void DeactivateItemMenu()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 100);
        var stock = faker.Random.Int(1, 100);
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        var itemMenu = new ItemMenu(name, description, price, stock, ingredients, size);

        itemMenu.Deactivate();

        itemMenu.Should()
                .Match<ItemMenu>(i => i.IsActive == false)
            .And.NotBeNull();
    }

    [Fact]
    public void UpdateItemMenu()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 100);
        var stock = faker.Random.Int(1, 100);
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        var itemMenu = new ItemMenu(name, description, price, stock, ingredients, size);

        var newName = faker.Commerce.ProductName();
        var newDescription = faker.Commerce.ProductDescription();
        var newPrice = faker.Random.Decimal(1, 100);
        var newStock = faker.Random.Int(1, 100);
        var newIngredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var newSize = Size.M;

        var newItemMenu = new ItemMenu(newName, newDescription, newPrice, newStock, newIngredients, newSize);

        itemMenu = itemMenu.Update(newItemMenu);

        itemMenu.Should()
                .Match<ItemMenu>(i => i.Name == newName)
            .And.Match<ItemMenu>(i => i.Description == newDescription)
            .And.Match<ItemMenu>(i => i.Price == newPrice)
            .And.Match<ItemMenu>(i => i.Stock == newStock)
            .And.Match<ItemMenu>(i => i.Ingredients == newIngredients)
            .And.Match<ItemMenu>(i => i.Size == newSize)
            .And.NotBeNull();
    }

    [Fact]
    public void CheckIfItemMenuIsUnavailable()
    {
        var faker = new Faker("pt_BR");
        var name = faker.Commerce.ProductName();
        var description = faker.Commerce.ProductDescription();
        var price = faker.Random.Decimal(1, 100);
        var stock = 1;
        var ingredients = new List<Ingredient>
        {
            new()
            {
                Name = faker.Commerce.ProductMaterial(),
                Calories = faker.Random.Int(1, 100)
            }
        };
        var size = Size.M;

        var itemMenu = new ItemMenu(name, description, price, stock, ingredients, size);

        itemMenu.Deactivate();

        itemMenu.Should()
                .Match<ItemMenu>(i => i.IsActive == false)
            .And.NotBeNull();
    }
}
