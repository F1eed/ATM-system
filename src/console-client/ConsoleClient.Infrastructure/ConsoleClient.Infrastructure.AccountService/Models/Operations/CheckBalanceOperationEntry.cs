namespace ConsoleClient.Infrastructure.AccountService.Models.Operations;

public sealed record CheckBalanceOperationEntry(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal Balance) : BaseOperationEntry(OperationId, AccountId, OccuredAt);