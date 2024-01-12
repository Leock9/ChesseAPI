﻿using Bogus;
using Domain.Base;
using Domain.ValueObjects;
using FluentAssertions;

namespace Domain.Tests;

public class OrderTests
{
    [Fact]
    public void CreateOrder()
    {
        var faker = new Faker("pt_BR");
        var clientId = Guid.NewGuid();
        var totalOrder = faker.Random.Decimal(1, 100);

        var order = new Order(totalOrder, clientId);

        order.Should()
             .Match<Order>(o => o.TotalOrder == totalOrder)
             .And.Match<Order>(o => o.ClientId == clientId)
             .And.Match<Order>(o => o.Status == Status.Received)
             .And.NotBeNull();
    }

    [Fact]
    public void CreateOrderWhenTotalOrderIsZero()
    {
        var clientId = Guid.NewGuid();
        var totalOrder = 0;

        Action action = () =>
        {
            new Order(totalOrder, clientId);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Total order is required");
    }

    [Fact]
    public void CreateOrderWhenClientIdIsEmpty()
    {
        var clientId = Guid.Empty;
        var totalOrder = 10;

        Action action = () =>
        {
            new Order(totalOrder, clientId);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Client Id is required");
    }

    [Fact]
    public void ChangeOrderStatusToPreparing()
    {
        var faker = new Faker("pt_BR");
        var clientId = Guid.NewGuid();
        var totalOrder = faker.Random.Decimal(1, 100);
        var order = new Order(totalOrder, clientId);

        order = order.ChangeStatus(Status.Preparation);

        order.Should()
             .Match<Order>(o => o.Status == Status.Preparation)
             .And.NotBeNull();
    }

    [Fact]
    public void ChangeOrderStatusToInDelivery()
    {
        var faker = new Faker("pt_BR");
        var clientId = Guid.NewGuid();
        var totalOrder = faker.Random.Decimal(1, 100);
        var order = new Order(totalOrder, clientId);

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
        var clientId = Guid.NewGuid();
        var totalOrder = faker.Random.Decimal(1, 100);
        var order = new Order(totalOrder, clientId);

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
        var clientId = Guid.NewGuid();
        var totalOrder = faker.Random.Decimal(1, 100);
        var order = new Order(totalOrder, clientId);

        Action action = () =>
        {
            order = order.ChangeStatus(Status.Received);
        };

        action.Should()
              .Throw<DomainException>()
              .WithMessage("Status cannot be changed to received");
    }
}
