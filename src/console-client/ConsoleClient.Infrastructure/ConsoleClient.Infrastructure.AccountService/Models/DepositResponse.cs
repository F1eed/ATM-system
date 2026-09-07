namespace ConsoleClient.Infrastructure.AccountService.Models;

public record DepositResponse(DepositResponse.AccountEntry Entry)
{
    public record AccountEntry(string CardNumber, decimal Balance);
}