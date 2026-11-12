namespace ConsoleClient.Application.Model.Operations;

public sealed record DepositOperation(
    DateTimeOffset OccuredAt,
    long OperationId,
    long AccountId,
    decimal Amount,
    decimal RemainingBalance) : IOperation;