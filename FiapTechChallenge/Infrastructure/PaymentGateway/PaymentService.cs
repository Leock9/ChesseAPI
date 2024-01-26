
using Domain.Entity.Payment;
using Domain.Services;

namespace Infrastructure.PaymentGateway;

public class PaymentService : IPaymentService
{
    public Task<bool> PayAsync(Payment payment)
    {
        Task.Delay(500);
        return Task.FromResult(true);
    }
}
