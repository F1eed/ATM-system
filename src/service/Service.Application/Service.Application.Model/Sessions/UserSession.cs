using Service.Application.Model.Accounts;

namespace Service.Application.Model.Sessions;

public class UserSession : ISession
{
    private UserSession(AccountId accountId, Guid sessionId)
    {
        SessionId = sessionId;
        AccountId = accountId;
    }

    public Guid SessionId { get; }

    public AccountId AccountId { get; }

    public static UserSession CreateNew(AccountId accountId) => new(accountId, Guid.NewGuid());

    public static UserSession CreateFromDb(AccountId accountId, Guid sessionId) => new(accountId, sessionId);
}