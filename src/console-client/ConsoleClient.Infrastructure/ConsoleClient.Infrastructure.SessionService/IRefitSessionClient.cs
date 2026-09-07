using ConsoleClient.Infrastructure.SessionService.Models;
using Refit;

namespace ConsoleClient.Infrastructure.SessionService;

public interface IRefitSessionClient
{
    [Post("/api/session/admin")]
    Task<IApiResponse<CreateAdminResponse>> CreateAdminAsync(
        [Body] CreateAdminRequest request,
        CancellationToken cancellationToken);

    [Post("/api/session/user")]
    Task<IApiResponse<CreateUserResponse>> CreateUserAsync(
        [Body] CreateUserRequest request,
        CancellationToken cancellationToken);
}