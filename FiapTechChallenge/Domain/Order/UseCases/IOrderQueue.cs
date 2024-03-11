namespace Domain.Services;

public interface IOrderQueue
{
    public void Publish(Order order);
}