namespace ConsoleClient.Application.Contracts.Models.Operations;

public sealed record WithdrawOperationDto(
    DateTimeOffset OccuredAt,
    decimal Amount) : BaseOperationDto(OccuredAt);