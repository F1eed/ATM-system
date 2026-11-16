using Service.Application.Contracts.Session.Operations;

namespace Service.Application.Contracts.Session;

public interface ISessionService
{
    Task<CreateAdminSession.Response> CreateAdminSessionAsync(
        CreateAdminSession.Request request,
        CancellationToken cancellationToken);

    Task<CreateUserSession.Response> CreateUserSessionAsync(
        CreateUserSession.Request request,
        CancellationToken cancellationToken);
}