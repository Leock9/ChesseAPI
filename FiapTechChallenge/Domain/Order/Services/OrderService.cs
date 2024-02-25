
using Domain.Entity.Payment;
using Domain.Ports;
using Domain.Services.Requests;
using Microsoft.Extensions.Logging;

namespace Domain.Services;

public class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly IOrderQueue _queue;

    public OrderService
    (
        ILogger<OrderService> logger, 
        IOrderRepository orderRepository, 
        IPaymentService paymentService,
        IOrderQueue queue
    )
    {
        _logger = logger;
        _orderRepository = orderRepository;
        _paymentService = paymentService;
        _queue = queue; 
    }

    public Guid CreateOrder(BaseOrderRequest orderRequest)
    {
        try
        {
            if (!Task.Run(() => _paymentService.PayAsync(new Payment(orderRequest.TotalOrder))).Result)
                throw new Exception("Payment failed");

            var order = new Order
            (
             orderRequest.TotalOrder,
             orderRequest.Document,
             orderRequest.ItemMenuIds
            );

            _orderRepository.Create(order);
            _queue.Publish(order);

            return order.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public Task<IEnumerable<Order>> GetAll()
    {
        try
        {
            return _orderRepository.GetAll();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public void UpdateStatusOrderAsync(Order order)
    {
        throw new NotImplementedException();
    }
}
