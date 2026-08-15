using Service.Application.Model.Accounts;

namespace Service.Application.Model.Sessions;

public class UserSession : ISession
{
    public UserSession(AccountId accountId)
    {
        SessionId = Guid.NewGuid();
        AccountId = accountId;
    }

    public Guid SessionId { get; }

    public AccountId AccountId { get; }
}