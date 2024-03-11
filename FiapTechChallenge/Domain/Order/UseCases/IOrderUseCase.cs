using Domain.Services.Requests;

namespace Domain.Services;

public interface IOrderUseCase
{
    public Guid CreateOrder(BaseOrderRequest orderRequest);
    public Task<IEnumerable<Order>> GetAll();
    public Task UpdateStatusOrderAsync(UpdateOrderStatusRequest orderRequest);
}
