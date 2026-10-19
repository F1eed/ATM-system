namespace Service.Presentation.Http.Models;

public sealed record DepositResponse(DepositResponse.AccountEntry Entry)
{
    public sealed record AccountEntry(
        string CardNumber,
        decimal Amount,
        decimal Balance);
}