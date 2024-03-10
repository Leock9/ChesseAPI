using Bogus;
using Bogus.Extensions.Brazil;
using Domain.Ports;
using Domain.Services;
using Domain.Services.Requests;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Domain.Tests.Service;

public class OrderServiceTests
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly IOrderQueue _queue;

    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _logger = new Mock<ILogger<OrderService>>().Object;
        _orderRepository = new Mock<IOrderRepository>().Object;
        _paymentService = new Mock<IPaymentService>().Object;
        _queue = new Mock<IOrderQueue>().Object;

        _orderService = new OrderService
            (
              _logger,
              _orderRepository,
              _paymentService,
              _queue
            );
    }

    [Fact]
    public void CreateOrderAsyncWhenOrderIsValidPayedSuccesShouldReturnOrder()
    {
        var faker = new Faker("pt_BR");

        var request = new BaseOrderRequest
        (
           faker.Random.Decimal(1, 100),
           faker.Person.Cpf(),
           faker.Make(10, () => Guid.NewGuid().ToString())
        );

        var payment = new Payment(request.TotalOrder)
        {
            IsAproved = true
        };

        Mock.Get(_paymentService).Setup(x => x.PayAsync(It.IsAny<Payment>())).Returns(payment);
        Mock.Get(_orderRepository).Setup(x => x.Create(It.IsAny<Order>()));

        var result = _orderService.CreateOrder(request);

        Mock.Get(_orderRepository).Verify(x => x.Create(It.Is<Order>(o => o.Document == request.Document &&
                                                                                            o.TotalOrder == request.TotalOrder &&
                                                                                            o.ItemMenuIds == request.ItemMenuIds &&
                                                                                            o.Status == ValueObjects.Status.Received)), Times.Once);
        Mock.Get(_queue).Verify(x => x.Publish(It.IsAny<Order>()), Times.Once);

        result.Should()
              .NotBeEmpty();
    }

    [Fact]
    public void CreateOrderAsyncWhenOrderIsValidButNotPayedShouldReturnOrder()
    {
        var faker = new Faker("pt_BR");
        
        var request = new BaseOrderRequest
        (
           faker.Random.Decimal(1, 100),
           faker.Person.Cpf(),
           faker.Make(10, () => Guid.NewGuid().ToString())
        );


        var payment = new Payment(request.TotalOrder)
        {
            IsAproved = false
        };

        Mock.Get(_paymentService).Setup(x => x.PayAsync(It.IsAny<Payment>())).Returns(payment);
        Mock.Get(_orderRepository).Setup(x => x.Create(It.IsAny<Order>()));

        var result = _orderService.CreateOrder(request);

        Mock.Get(_orderRepository).Verify(x => x.Create(It.Is<Order>(o => o.Document == request.Document &&
                                                                                            o.TotalOrder == request.TotalOrder &&
                                                                                            o.ItemMenuIds == request.ItemMenuIds &&
                                                                                            o.Status == ValueObjects.Status.PaymentPending)), Times.Once);
        Mock.Get(_queue).Verify(x => x.Publish(It.IsAny<Order>()), Times.Never);


        result.Should()
              .NotBeEmpty();
    }

    [Fact]
    public void GetAllAsyncWhenHasOrdersShouldReturnOrdersOrderBy()
    {
        var faker = new Faker("pt_BR");

        var orders = faker.Make(10, () => new Order
               (
                faker.Random.Decimal(1, 100),
                faker.Person.Cpf(),
                faker.Make(10, () => Guid.NewGuid().ToString()),
                new Payment(faker.Random.Decimal(1, 100))
                ));

        Mock.Get(_orderRepository).Setup(x => x.GetAll()).ReturnsAsync(orders);

        var result = _orderService.GetAll().Result;

        result.Should()
              .NotBeEmpty()
              .And
              .HaveCount(10);
    }

    [Fact]
    public async Task UpdateStatusOrderAsyncWhenOrderIsValidShouldReturnOrder()
    {
        var faker = new Faker("pt_BR");
        var request = new UpdateOrderStatusRequest(Guid.NewGuid(), (int)ValueObjects.Status.Preparation);

        var order = new Order
            (
             faker.Random.Decimal(1, 100),
             faker.Person.Cpf(),
             faker.Make(10, () => Guid.NewGuid().ToString()),
             new Payment(faker.Random.Decimal(1, 100))
            );

        order.Status = (ValueObjects.Status.Received);
        order.Payment.IsAproved = true;

        Mock.Get(_orderRepository).Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(order);
        Mock.Get(_orderRepository).Setup(x => x.UpdateAsync(It.IsAny<Order>()));

        await _orderService.UpdateStatusOrderAsync(request);

        Mock.Get(_orderRepository).Verify(x => x.UpdateAsync(It.Is<Order>(o => ((int)o.Status) == request.Status)), Times.Once);

        Mock.Get(_queue).Verify(x => x.Publish(It.Is<Order>(o => ((int)o.Status) == request.Status)), Times.Once);
    }

    [Fact]
    public async Task UpdateStatusOrderAsyncWhenOrderIsCanceled()
    {
        var faker = new Faker("pt_BR");
        var request = new UpdateOrderStatusRequest(Guid.NewGuid(), (int)ValueObjects.Status.Canceled);

        var order = new Order
            (
             faker.Random.Decimal(1, 100),
             faker.Person.Cpf(),
             faker.Make(10, () => Guid.NewGuid().ToString()),
             new Payment(faker.Random.Decimal(1, 100))
            );

        order.Status = (ValueObjects.Status.Received);
        order.Payment.IsAproved = true;

        Mock.Get(_orderRepository).Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(order);
        Mock.Get(_orderRepository).Setup(x => x.UpdateAsync(It.IsAny<Order>()));

        await _orderService.UpdateStatusOrderAsync(request);

        Mock.Get(_orderRepository).Verify(x => x.UpdateAsync(It.Is<Order>(o => ((int)o.Status) == request.Status)), Times.Once);

        Mock.Get(_queue).Verify(x => x.Publish(It.Is<Order>(o => ((int)o.Status) == request.Status)), Times.Once);
    }

    [Fact]
    public async Task UpdateStatusOrderAsyncWhenOrderPaymentoNotAproved()
    {
        var faker = new Faker("pt_BR");
        var request = new UpdateOrderStatusRequest(Guid.NewGuid(), (int)ValueObjects.Status.Preparation);

        var order = new Order
            (
             faker.Random.Decimal(1, 100),
             faker.Person.Cpf(),
             faker.Make(10, () => Guid.NewGuid().ToString()),
             new Payment(faker.Random.Decimal(1, 100))
            );

        order.Status = (ValueObjects.Status.Received);
        order.Payment.IsAproved = false;

        Mock.Get(_orderRepository).Setup(x => x.GetById(It.IsAny<Guid>())).ReturnsAsync(order);
        Mock.Get(_orderRepository).Setup(x => x.UpdateAsync(It.IsAny<Order>()));

        await _orderService.UpdateStatusOrderAsync(request);

        Mock.Get(_orderRepository).Verify(x => x.UpdateAsync(It.Is<Order>(o => (o.Status) == ValueObjects.Status.PaymentPending)), Times.Once);

        Mock.Get(_queue).Verify(x => x.Publish(It.Is<Order>(o => (o.Status) == ValueObjects.Status.PaymentPending)), Times.Once);
    }
}
