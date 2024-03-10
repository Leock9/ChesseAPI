namespace Infrastructure.PaymentGateway.Webhook;

public interface IPaymentWebHook
{
    Task<PaymentHook> PaymentHookAsync(Guid transactionId);
}
