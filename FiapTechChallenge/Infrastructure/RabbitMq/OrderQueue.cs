using Domain;
using Domain.Services;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.RabbitMq;

public class OrderQueue : IOrderQueue
{
    private readonly IRabbitMqSettings _rabbitMqSettings;
    private readonly ConnectionFactory _connectionFactory;

    public OrderQueue(IRabbitMqSettings rabbitMqSettings)
    {
        _rabbitMqSettings = rabbitMqSettings;

        _connectionFactory = new ConnectionFactory
        {
            HostName = _rabbitMqSettings.HostName,
            UserName = _rabbitMqSettings.UserName,
            Password = _rabbitMqSettings.Password,
            VirtualHost = "/"
        };
    }

    public void Publish(Order order)
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare
        (
         queue: $"order_{order.Status}",
         durable: false,
         exclusive: false,
         autoDelete: false,
         arguments: null
        );

        channel.BasicPublish
        (
            exchange: "",
            routingKey: $"order_{order.Status}",
            basicProperties: null,
            body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(order))
         );
    }
}