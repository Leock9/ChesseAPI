using Domain.Base;
using Domain.ValueObjects;

namespace Domain;

public record Order(decimal TotalOrder, Guid ClientId)
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public decimal TotalOrder { get; init; } = TotalOrder <= 0 ? 
                                               throw new DomainException("Total order is required") : TotalOrder;

    public Status Status { get; init; } = Status.Received;

    public  Guid ClientId { get; init; } = ClientId == Guid.Empty ?
                                           throw new DomainException("Client Id is required") : ClientId;

    public DateTime CreatedAt { get; init; } = DateTime.Now;

    public DateTime UpdatedAt { get; init;} = DateTime.Now;

    public Order ChangeStatus(Status newStatus)
    {
        if (newStatus == Status.Received)
            throw new DomainException("Status cannot be changed to received");

        if (newStatus == Status.Preparation && Status != Status.Received)
            throw new DomainException("Status cannot be changed to preparing");

        if (newStatus == Status.Ready && Status != Status.Preparation)
            throw new DomainException("Status cannot be changed to in delivery");

        if (newStatus == Status.Finished && Status != Status.Ready)
            throw new DomainException("Status cannot be changed to delivered");

        return this with { Status = newStatus };
    }
} 
