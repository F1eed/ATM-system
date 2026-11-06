namespace Service.Application.Contracts.Operation.Models;

public record WithdrawOperationDto(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    decimal Amount,
    decimal RemainingBalance) : BaseOperationDto(OperationId, AccountId, OccuredAt);