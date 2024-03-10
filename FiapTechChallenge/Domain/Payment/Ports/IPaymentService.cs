namespace Domain.Services;

public interface IPaymentService
{
    public Payment PayAsync(Payment payment);

    public void AprovePayment(Payment payment);
}
