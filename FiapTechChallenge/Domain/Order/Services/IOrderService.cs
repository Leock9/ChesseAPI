using Domain.Services.Requests;

namespace Domain.Services;

public interface IOrderService
{
    public Guid CreateOrder(BaseOrderRequest orderRequest);
    public Task<IEnumerable<Order>> GetAll();
    public Task UpdateStatusOrderAsync(UpdateOrderStatusRequest orderRequest);
}
