namespace ConsoleClient.Infrastructure.AccountService.Models;

public sealed record CheckHistoryResponse(
    IReadOnlyCollection<CheckHistoryResponse.OperationEntry> Operations)
{
    public sealed record OperationEntry(
        long OperationId,
        long AccountId,
        DateTimeOffset OccuredAt,
        string Type,
        decimal? Amount,
        decimal RemainingBalance);
}