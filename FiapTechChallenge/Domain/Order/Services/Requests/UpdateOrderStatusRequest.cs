namespace Domain.Services.Requests;
public record UpdateOrderStatusRequest(Guid OrderId, int Status);
