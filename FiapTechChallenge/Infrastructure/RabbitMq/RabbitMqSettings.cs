namespace Infrastructure.RabbitMq;

public record RabbitMqSettings
{
    public string HostName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public int Port { get; init; } = 0;
}
