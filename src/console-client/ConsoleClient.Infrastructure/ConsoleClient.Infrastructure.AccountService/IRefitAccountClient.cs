using ConsoleClient.Infrastructure.AccountService.Models;
using Refit;

namespace ConsoleClient.Infrastructure.AccountService;

public interface IRefitAccountClient
{
    [Post("/api/account")]
    Task<IApiResponse<CreateAccountResponse>> CreateAccountAsync(
        [Body] CreateAccountRequest request,
        CancellationToken cancellationToken);

    [Post("/api/account/{sessionId}/deposits")]
    Task<IApiResponse<DepositResponse>> DepositMoneyAsync(
        Guid sessionId,
        [Body] DepositRequest request,
        CancellationToken cancellationToken);

    [Post("/api/account/{sessionId}/withdraws")]
    Task<IApiResponse<WithdrawResponse>> WithdrawMoneyAsync(
        Guid sessionId,
        [Body] WithdrawRequest request,
        CancellationToken cancellationToken);

    [Get("/api/account/{sessionId}/balance")]
    Task<IApiResponse<CheckBalanceResponse>> CheckBalanceAsync(
        Guid sessionId,
        CancellationToken cancellationToken);

    [Get("/api/account/{sessionId}/operations")]
    Task<IApiResponse<CheckHistoryResponse>> CheckHistoryAsync(
        [Query] Guid sessionId,
        [Query] CheckHistoryRequest request,
        CancellationToken cancellationToken);
}