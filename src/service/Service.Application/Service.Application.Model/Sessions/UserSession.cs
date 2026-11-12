using Service.Application.Model.Accounts;

namespace Service.Application.Model.Sessions;

public class UserSession : ISession
{
    private UserSession(AccountId accountId, Guid sessionId)
    {
        SessionId = new SessionId(sessionId);
        AccountId = accountId;
    }

    public AccountId AccountId { get; }

    public SessionId SessionId { get; }

    public static UserSession CreateNew(AccountId accountId)
    {
        return new UserSession(accountId, Guid.NewGuid());
    }

    public static UserSession CreateFromDb(AccountId accountId, Guid sessionId)
    {
        return new UserSession(accountId, sessionId);
    }
}