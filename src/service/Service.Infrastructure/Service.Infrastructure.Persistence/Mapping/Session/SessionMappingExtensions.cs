using Service.Application.Model.Accounts;
using Service.Application.Model.Sessions;
using Service.Infrastructure.Persistence.Models.Session;
using System.Diagnostics;

namespace Service.Infrastructure.Persistence.Mapping;

public static class SessionMappingExtensions
{
    public static ISession ToDomain(this SessionModel session)
    {
        return session switch
        {
            AdminSessionModel adminSession => AdminSession.CreateFromDb(adminSession.SessionId),
            UserSessionModel userSession => UserSession
                .CreateFromDb(new AccountId(userSession.AccountId), userSession.SessionId),
            _ => throw new UnreachableException(),
        };
    }

    public static SessionModel ToDbModel(this ISession session)
    {
        return session switch
        {
            AdminSession adminSession => new AdminSessionModel(adminSession.SessionId),
            UserSession userSession => new UserSessionModel(
                userSession.SessionId,
                userSession.AccountId.Value),
            _ => throw new UnreachableException(),
        };
    }
}