namespace Domain.Services.Requests;

public record CreateClientRequest(string Name, string Document, string Email);