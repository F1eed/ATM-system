namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.CheckBalance;

public sealed record CheckBalanceOperationEntry(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal RemainingBalance) : BaseOperationEntry(OperationId, AccountId, OccuredAt);