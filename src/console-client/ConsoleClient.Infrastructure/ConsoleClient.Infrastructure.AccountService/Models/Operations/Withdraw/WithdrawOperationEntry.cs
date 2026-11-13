namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.Withdraw;

public sealed record WithdrawOperationEntry(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal Amount) : BaseOperationEntry(OperationId, AccountId, OccuredAt);