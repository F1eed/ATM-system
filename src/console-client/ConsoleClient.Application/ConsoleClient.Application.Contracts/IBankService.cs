using ConsoleClient.Application.Contracts.Operations;

namespace ConsoleClient.Application.Contracts;

public interface IBankService
{
    Task<CreateUserSession.Result> CreateUserSessionAsync(
        CreateUserSession.Request request,
        CancellationToken cancellationToken);

    Task<CreateAdminSession.Result> CreateAdminSessionAsync(
        CreateAdminSession.Request request,
        CancellationToken cancellationToken);
}