namespace ConsoleClient.Infrastructure.AccountService.Models.Operations;

public sealed record CheckBalanceOperationEntry(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal RemainingBalance) : BaseOperationEntry(OperationId, AccountId, OccuredAt);