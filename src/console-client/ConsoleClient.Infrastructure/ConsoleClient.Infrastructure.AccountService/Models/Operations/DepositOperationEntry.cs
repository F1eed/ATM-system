namespace ConsoleClient.Infrastructure.AccountService.Models.Operations;

public sealed record DepositOperationEntry(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal Amount) : BaseOperationEntry(OperationId, AccountId, OccuredAt);