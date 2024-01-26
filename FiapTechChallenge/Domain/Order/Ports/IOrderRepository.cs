namespace Domain.Ports;

public interface IOrderRepository
{
    public void Create(Order order);
    public Task<IEnumerable<Order>> GetAll();
    public void UpdateAsync(Order order);
}
