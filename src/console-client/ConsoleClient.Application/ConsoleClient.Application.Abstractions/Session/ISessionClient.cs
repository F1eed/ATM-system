using ConsoleClient.Application.Abstractions.Session.Operations;

namespace ConsoleClient.Application.Abstractions.Session;

public interface ISessionClient
{
    Task<CreateAdminSession.Result> CreateAdminSessionAsync(
        CreateAdminSession.Request request,
        CancellationToken cancellationToken);

    Task<CreateUserSession.Result> CreateUserSessionAsync(
        CreateUserSession.Request request,
        CancellationToken cancellationToken);
}