namespace ConsoleClient.Application.Contracts.Models.Operations;

public sealed record WithdrawOperationDto(
    DateTimeOffset OccuredAt,
    decimal Amount,
    decimal RemainingBalance) : BaseOperationDto(OccuredAt);