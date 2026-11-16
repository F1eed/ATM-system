namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.CheckHistory;

public sealed record CheckHistoryResponse(
    IReadOnlyCollection<BaseOperationEntry> Operations,
    string? PageToken);