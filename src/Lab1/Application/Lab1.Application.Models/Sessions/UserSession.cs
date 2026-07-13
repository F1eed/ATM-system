using Lab1.Application.Models.Accounts;

namespace Lab1.Application.Models.Sessions;

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