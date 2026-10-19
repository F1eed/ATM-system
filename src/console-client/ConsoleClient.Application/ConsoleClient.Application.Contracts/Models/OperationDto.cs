namespace ConsoleClient.Application.Contracts.Models;

public sealed record OperationDto(DateTimeOffset OccuredAt, string Type, decimal? Amount, decimal Balance);