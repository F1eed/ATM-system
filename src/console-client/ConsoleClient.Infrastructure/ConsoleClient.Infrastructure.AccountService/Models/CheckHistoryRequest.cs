namespace ConsoleClient.Infrastructure.AccountService.Models;

public sealed record CheckHistoryRequest(
    string? PageToken,
    int PageSize);