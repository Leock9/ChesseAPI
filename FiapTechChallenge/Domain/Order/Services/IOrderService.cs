using Domain.Services.Requests;

namespace Domain.Services;

public interface IOrderService
{
    public void CreateOrder(BaseOrderRequest orderRequest);
    public Task<IEnumerable<Order>> GetAll();
    public void UpdateStatusOrderAsync(Order order);
}
