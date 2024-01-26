using Domain.Base;

namespace Domain.Entity.Payment;

public record Payment(decimal TotalOrder)
{

    public Guid Id { get; init; } = Guid.NewGuid();

    public decimal TotalOrder { get; init; } = TotalOrder < 0 ?
                                               throw new DomainException("Total order is required") : TotalOrder;
}
