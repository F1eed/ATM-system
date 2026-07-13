using Lab1.Application.Contracts.Session.Operations;

namespace Lab1.Application.Contracts.Session;

public interface ISessionService
{
    CreateAdminSession.Response CreateAdminSession(CreateAdminSession.Request request);

    CreateUserSession.Response CreateUserSession(CreateUserSession.Request request);
}