namespace Service.Application.Model.Sessions;

public class AdminSession : ISession
{
    private AdminSession(Guid sessionId)
    {
        SessionId = new SessionId(sessionId);
    }

    public SessionId SessionId { get; }

    public static AdminSession CreateNew()
    {
        return new AdminSession(Guid.NewGuid());
    }

    public static AdminSession CreateFromDb(Guid sessionId)
    {
        return new AdminSession(sessionId);
    }
}