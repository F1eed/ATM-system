using ConsoleClient.Application.Abstractions.Session.Operations;

namespace ConsoleClient.Application.Abstractions.Session;

public interface ISessionClient
{
    Task<ServiceCreateAdminSession.Result> CreateAdminSessionAsync(
        ServiceCreateAdminSession.Request request,
        CancellationToken cancellationToken);

    Task<ServiceCreateUserSession.Result> CreateUserSessionAsync(
        ServiceCreateUserSession.Request request,
        CancellationToken cancellationToken);
}