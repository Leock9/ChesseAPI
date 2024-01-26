using Domain.Entity.Payment;

namespace Domain.Services;

public interface IPaymentService
{
    public Task<bool> PayAsync(Payment payment);
}
