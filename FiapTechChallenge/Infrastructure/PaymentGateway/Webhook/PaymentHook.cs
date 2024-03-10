namespace Infrastructure.PaymentGateway.Webhook;

public record PaymentHook
{
    public Guid TransactionId { get; init; }
    public bool IsAproved { get; init; }
}
