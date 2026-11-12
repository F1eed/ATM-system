namespace ConsoleClient.Application.Model.Operations;

public sealed record CheckBalanceOperation(
    DateTimeOffset OccuredAt,
    long OperationId,
    long AccountId,
    decimal Balance) : IOperation;