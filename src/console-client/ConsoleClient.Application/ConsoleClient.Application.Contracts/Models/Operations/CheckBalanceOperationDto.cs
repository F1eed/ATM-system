namespace ConsoleClient.Application.Contracts.Models.Operations;

public sealed record CheckBalanceOperationDto(
    DateTimeOffset OccuredAt,
    decimal Balance) : BaseOperationDto(OccuredAt);