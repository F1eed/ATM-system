using ConsoleClient.Infrastructure.AccountService.Models;
using Refit;

namespace ConsoleClient.Infrastructure.AccountService;

public interface IRefitAccountClient
{
    [Post("/api/account")]
    Task<IApiResponse> CreateAccountAsync(
        [Body] CreateAccountRequest request,
        CancellationToken cancellationToken);
}