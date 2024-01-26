using Domain;
using Domain.Services;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.RabbitMq;

public class OrderQueue : IOrderQueue
{
    private readonly IModel _channel;

    public OrderQueue
    (
        IModel channel
    )
    {
        _channel = channel;

        _channel.QueueDeclare
        (
         queue: "order_queue",
         durable: false,
         exclusive: false,
         autoDelete: false,
         arguments: null
        );
    }

    public List<Order> Consume()
    {
        throw new NotImplementedException();
    }

    public void Publish(Order order)
    {
        _channel.BasicPublish
        (
            exchange: "",
            routingKey: "order_queue",
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order))
         );
    }
}