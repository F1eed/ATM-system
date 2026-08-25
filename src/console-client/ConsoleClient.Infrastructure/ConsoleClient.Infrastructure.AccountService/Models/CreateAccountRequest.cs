namespace ConsoleClient.Infrastructure.AccountService.Models;

public sealed record CreateAccountRequest(string CardNumber, string PinCode);
