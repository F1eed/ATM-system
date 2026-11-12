namespace ConsoleClient.Infrastructure.AccountService.Models.Operations;

public sealed record WithdrawOperationEntry(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal Amount,
    decimal RemainingBalance) : BaseOperationEntry(OperationId, AccountId, OccuredAt);