namespace ConsoleClient.Infrastructure.SessionService.Models;

public sealed record CreateUserRequest(string CardNumber, string PinCode);
