namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.Deposit;

public record DepositResponse(DepositResponse.AccountEntry Entry)
{
    public record AccountEntry(
        string CardNumber,
        decimal Amount,
        decimal Balance);
}