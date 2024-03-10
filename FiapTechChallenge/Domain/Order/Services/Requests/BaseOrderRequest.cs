namespace Domain.Services.Requests;

public record BaseOrderRequest
(
    decimal TotalOrder,
    string Document,
    IList<string> ItemMenuIds
);
