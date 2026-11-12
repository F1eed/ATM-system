using ConsoleClient.Infrastructure.AccountService.Models.Operations;

namespace ConsoleClient.Infrastructure.AccountService.Models;

public sealed record CheckHistoryResponse(
    IReadOnlyCollection<BaseOperationEntry> Operations,
    string? PageToken);