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
            var payment = new Payment(orderRequest.TotalOrder);
             payment = _paymentService.PayAsync(payment);

            var order = new Order
            (
             orderRequest.TotalOrder,
             orderRequest.Document,
             orderRequest.ItemMenuIds,
             payment
            );

            _orderRepository.Create(order);

            if(order.Status == ValueObjects.Status.Received && payment.IsAproved) 
                _queue.Publish(order);

            return order.Id;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<Order>> GetAll()
    {
        try
        {
            var orders =  await _orderRepository.GetAll();

            return orders
                  .OrderBy(x => x.CreatedAt)
                  .Where(x => x.Status != ValueObjects.Status.Finished);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    public async Task UpdateStatusOrderAsync(UpdateOrderStatusRequest orderRequest)
    {
        try
        {
            var order = await _orderRepository.GetById(orderRequest.OrderId) ??
                throw new NullReferenceException("Order not found");

            order = order.ChangeStatus((ValueObjects.Status)orderRequest.Status);
            await _orderRepository.UpdateAsync(order);
            _logger.LogInformation($"Order {order.Id} status changed to {order.Status}");

            _queue.Publish(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}
