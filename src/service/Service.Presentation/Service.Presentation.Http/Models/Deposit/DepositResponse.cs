namespace Service.Presentation.Http.Models.Deposit;

public sealed record DepositResponse(DepositResponse.DepositAccountEntry Entry)
{
    public sealed record DepositAccountEntry(
        string CardNumber,
        decimal Amount,
        decimal Balance);
}