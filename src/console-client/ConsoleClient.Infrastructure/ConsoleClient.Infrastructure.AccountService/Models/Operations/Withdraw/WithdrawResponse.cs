namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.Withdraw;

public sealed record WithdrawResponse(WithdrawResponse.AccountEntry Entry)
{
    public sealed record AccountEntry(
        string CardNumber,
        decimal Amount,
        decimal Balance);
}