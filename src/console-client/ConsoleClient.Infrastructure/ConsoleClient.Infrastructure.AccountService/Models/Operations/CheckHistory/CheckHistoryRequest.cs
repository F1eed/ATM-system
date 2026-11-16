namespace ConsoleClient.Infrastructure.AccountService.Models.Operations.CheckHistory;

public sealed record CheckHistoryRequest(
    string? PageToken,
    int PageSize);