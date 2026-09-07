namespace ConsoleClient.Infrastructure.AccountService.Models;

public sealed record CheckHistoryResponse(IReadOnlyCollection<CheckHistoryResponse.OperationEntry> Operations)
{
    public sealed record OperationEntry(long OperationId, long AccountId, string Type, decimal Balance);
}