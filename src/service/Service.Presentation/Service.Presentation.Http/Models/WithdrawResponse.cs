namespace Service.Presentation.Http.Models;

public sealed record WithdrawResponse(WithdrawResponse.AccountEntry Entry)
{
    public sealed record AccountEntry(
        string CardNumber,
        decimal Amount,
        decimal Balance);
}
