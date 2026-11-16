namespace ConsoleClient.Application.Contracts.Models.Operations;

public sealed record DepositOperationDto(
    DateTimeOffset OccuredAt,
    decimal Amount) : BaseOperationDto(OccuredAt);