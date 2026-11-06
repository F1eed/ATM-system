namespace ConsoleClient.Infrastructure.AccountService.Models;

public sealed record CheckHistoryResponse(
    IReadOnlyCollection<CheckHistoryResponse.BaseOperationEntry> Operations)
{
    public abstract record BaseOperationEntry(
        long OperationId,
        long AccountId,
        DateTimeOffset OccuredAt);

    public sealed record DepositOperationEntry(
        long OperationId,
        long AccountId,
        DateTimeOffset OccuredAt,
        decimal Amount,
        decimal RemainingBalance) : BaseOperationEntry(OperationId, AccountId, OccuredAt);

    public sealed record WithdrawOperationEntry(
        long OperationId,
        long AccountId,
        DateTimeOffset OccuredAt,
        decimal Amount,
        decimal RemainingBalance) : BaseOperationEntry(OperationId, AccountId, OccuredAt);

    public sealed record CheckBalanceOperationEntry(
        long OperationId,
        long AccountId,
        DateTimeOffset OccuredAt,
        decimal Amount,
        decimal RemainingBalance) : BaseOperationEntry(OperationId, AccountId, OccuredAt);
}