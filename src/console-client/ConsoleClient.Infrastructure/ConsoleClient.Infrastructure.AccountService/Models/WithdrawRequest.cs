namespace ConsoleClient.Infrastructure.AccountService.Models;

public sealed record WithdrawRequest(Guid SessionId, decimal Amount);