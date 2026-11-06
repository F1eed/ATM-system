using Service.Application.Model.Accounts;

namespace Service.Application.Model.Sessions;

public class UserSession : ISession
{
    private UserSession(AccountId accountId, Guid sessionId)
    {
        SessionId = new SessionId(sessionId);
        AccountId = accountId;
    }

    public SessionId SessionId { get; }

    public AccountId AccountId { get; }

    public static UserSession CreateNew(AccountId accountId) => new(accountId, Guid.NewGuid());

    public static UserSession CreateFromDb(AccountId accountId, Guid sessionId) => new(accountId, sessionId);
}