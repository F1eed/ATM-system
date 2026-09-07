namespace ConsoleClient.Infrastructure.AccountService.Models;

public record DepositRequest(Guid SessionId, decimal Amount);