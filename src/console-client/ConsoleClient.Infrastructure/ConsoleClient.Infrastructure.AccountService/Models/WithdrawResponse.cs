namespace ConsoleClient.Infrastructure.AccountService.Models;

public sealed record WithdrawResponse(WithdrawResponse.AccountEntry Entry)
{
    public sealed record AccountEntry(string CardNumber, decimal Balance);
}