namespace Domain.Services;

public interface IPaymentGateway
{
    public Payment PayAsync(Payment payment);

    public void AprovePayment(Payment payment);
}
