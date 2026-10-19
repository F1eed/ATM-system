namespace ConsoleClient.Application.Model;

public record Operation(
    long OperationId,
    long AccountId,
    DateTimeOffset OccuredAt,
    string Type,
    decimal? Amount,
    decimal RemainingBalance);