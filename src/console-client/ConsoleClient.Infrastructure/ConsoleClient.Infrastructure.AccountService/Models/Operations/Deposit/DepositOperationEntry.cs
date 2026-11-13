namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.Deposit;

public sealed record DepositOperationEntry(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal Amount) : BaseOperationEntry(OperationId, AccountId, OccuredAt);