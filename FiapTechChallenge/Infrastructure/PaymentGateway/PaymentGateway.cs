
using Domain;
using Domain.Services;
using Infrastructure.PaymentGateway.Webhook;
using Microsoft.Extensions.Logging;

namespace Infrastructure.PaymentGateway;

public class PaymentGateway : IPaymentGateway
{
    private readonly ILogger<PaymentGateway> _logger;
    private readonly IPaymentWebHook _paymentWebHook;

    public PaymentGateway
    (
        ILogger<PaymentGateway> logger,
        IPaymentWebHook paymentWebHook
    )
    {
        _logger = logger;
        _paymentWebHook = paymentWebHook;
    }

    public Payment PayAsync(Payment payment)
    {
        Task.Delay(500);

        var urlWebHoot = Guid.NewGuid();
        var paymentHook = _paymentWebHook.PaymentHookAsync(urlWebHoot).Result;

        if(paymentHook.IsAproved) AprovePayment(payment);

        _logger.LogInformation($"Payment {payment.Id} is {(payment.IsAproved ? "Aproved" : "Not Aproved")}");

        return payment;
    }

    public void AprovePayment(Payment payment) => payment.IsAproved = true;
}
