using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Base;
using Domain.ValueObjects;
using FluentAssertions;
using System.Reflection.Metadata;

namespace Domain.Tests;

public class OrderTests
{
    [Fact]
    public void CreateOrder()
    {
        var faker = new Faker("pt_BR");
        var document = faker.Person.Cpf();
        var totalOrder = faker.Random.Decimal(1, 100);
        var itemsMenu = new List<ItemMenu>
        {
            new
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                faker.Random.Decimal(1, 100),
                faker.Random.Int(1, 100),
                 new List<Ingredient>
                 {
                   new()
                   {
                       Name = faker.Commerce.ProductMaterial(),
                       Calories = faker.Random.Int(1, 100)
                   }
                 },
                 Size.M,
                 Category.Sandwich
            )
        };

        var order = new Order(totalOrder, document, itemsMenu);

        order.Should()
             .Match<Order>(o => o.TotalOrder == totalOrder)
             .And.Match<Order>(o => o.Document == document)
             .And.Match<Order>(o => o.Status == Status.Received)
             .And.NotBeNull();
    }

    [Fact]
    public void CreateOrderWhenTotalOrderIsZero()
    {
        var faker = new Faker("pt_BR");
        var document = faker.Person.Cpf();
        var totalOrder = 0;
        var itemsMenu = new List<ItemMenu>
        {
            new
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                faker.Random.Decimal(1, 100),
                faker.Random.Int(1, 100),
                 new List<Ingredient>
                 {
                   new()
                   {
                       Name = faker.Commerce.ProductMaterial(),
                       Calories = faker.Random.Int(1, 100)
                   }
                 },
                 Size.M,
                 Category.Sandwich
            )
        };

        Action action = () =>
        {
            new Order(totalOrder, document, itemsMenu);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Total order is required");
    }

    [Fact]
    public void CreateOrderWhenClientIdIsEmpty()
    {
        var faker = new Faker("pt_BR");
        var document = string.Empty;
        var totalOrder = 10;
        var itemsMenu = new List<ItemMenu>
        {
            new
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                faker.Random.Decimal(1, 100),
                faker.Random.Int(1, 100),
                 new List<Ingredient>
                 {
                   new()
                   {
                       Name = faker.Commerce.ProductMaterial(),
                       Calories = faker.Random.Int(1, 100)
                   }
                 },
                 Size.M,
                 Category.Sandwich
            )
        };

        Action action = () =>
        {
            new Order(totalOrder, document, itemsMenu);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Client Id is required");
    }

    [Fact]
    public void ChangeOrderStatusToPreparing()
    {
        var faker = new Faker("pt_BR");
        var document = faker.Person.Cpf();
        var totalOrder = faker.Random.Decimal(1, 100);
        var itemsMenu = new List<ItemMenu>
        {
            new
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                faker.Random.Decimal(1, 100),
                faker.Random.Int(1, 100),
                 new List<Ingredient>
                 {
                   new()
                   {
                       Name = faker.Commerce.ProductMaterial(),
                       Calories = faker.Random.Int(1, 100)
                   }
                 },
                 Size.M,
                 Category.Sandwich
            )
        };

        var order = new Order(totalOrder, document, itemsMenu);

        order = order.ChangeStatus(Status.Preparation);

        order.Should()
             .Match<Order>(o => o.Status == Status.Preparation)
             .And.NotBeNull();
    }

    [Fact]
    public void ChangeOrderStatusToInDelivery()
    {
        var faker = new Faker("pt_BR");
        var document = faker.Person.Cpf();
        var totalOrder = faker.Random.Decimal(1, 100);
        var itemsMenu = new List<ItemMenu>
        {
            new
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                faker.Random.Decimal(1, 100),
                faker.Random.Int(1, 100),
                 new List<Ingredient>
                 {
                   new()
                   {
                       Name = faker.Commerce.ProductMaterial(),
                       Calories = faker.Random.Int(1, 100)
                   }
                 },
                 Size.M,
                 Category.Sandwich
            )
        };

        var order = new Order(totalOrder, document, itemsMenu);

        order = order.ChangeStatus(Status.Preparation);
        order = order.ChangeStatus(Status.Ready);

        order.Should()
             .Match<Order>(o => o.Status == Status.Ready)
             .And.NotBeNull();
    }

    [Fact]
    public void ChangeOrderStatusToDelivered()
    {
        var faker = new Faker("pt_BR");
        var document = faker.Person.Cpf();
        var totalOrder = faker.Random.Decimal(1, 100);
        var itemsMenu = new List<ItemMenu>
        {
            new
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                faker.Random.Decimal(1, 100),
                faker.Random.Int(1, 100),
                 new List<Ingredient>
                 {
                   new()
                   {
                       Name = faker.Commerce.ProductMaterial(),
                       Calories = faker.Random.Int(1, 100)
                   }
                 },
                 Size.M,
                 Category.Sandwich
            )
        };

        var order = new Order(totalOrder, document, itemsMenu);

        order = order.ChangeStatus(Status.Preparation);
        order = order.ChangeStatus(Status.Ready);
        order = order.ChangeStatus(Status.Finished);

        order.Should()
             .Match<Order>(o => o.Status == Status.Finished)
             .And.NotBeNull();
    }

    [Fact]
    public void ChangeOrderStatusToReceived()
    {
        var faker = new Faker("pt_BR");
        var document = faker.Person.Cpf();
        var totalOrder = faker.Random.Decimal(1, 100);
        var itemsMenu = new List<ItemMenu>
        {
            new
            (
                faker.Commerce.ProductName(),
                faker.Commerce.ProductDescription(),
                faker.Random.Decimal(1, 100),
                faker.Random.Int(1, 100),
                 new List<Ingredient>
                 {
                   new()
                   {
                       Name = faker.Commerce.ProductMaterial(),
                       Calories = faker.Random.Int(1, 100)
                   }
                 },
                 Size.M,
                 Category.Sandwich
            )
        };

        var order = new Order(totalOrder, document, itemsMenu);

        Action action = () =>
        {
            order = order.ChangeStatus(Status.Received);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Status cannot be changed to received");
    }

    [Fact]
    public void CreateOrderWhenItemMenuIsEmpty()
    {
        var faker = new Faker("pt_BR");
        var document = faker.Person.Cpf();
        var totalOrder = faker.Random.Decimal(1, 100);
        var itemsMenu = new List<ItemMenu>();

        Action action = () =>
        {
            new Order(totalOrder, document, itemsMenu);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Item Menu is required");
    }   
}
