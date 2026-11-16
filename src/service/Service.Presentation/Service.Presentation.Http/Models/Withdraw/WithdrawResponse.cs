namespace Service.Presentation.Http.Models.Withdraw;

public sealed record WithdrawResponse(WithdrawResponse.WithdrawAccountEntry Entry)
{
    public sealed record WithdrawAccountEntry(
        string CardNumber,
        decimal Amount,
        decimal Balance);
}