namespace Service.Application.Contracts.Operation.Models;

public record CheckBalanceOperationDto(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal RemainingBalance) : BaseOperationDto(OperationId, AccountId, OccuredAt);
