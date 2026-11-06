namespace Service.Application.Model.Sessions;

public class AdminSession : ISession
{
    private AdminSession(Guid sessionId)
    {
        SessionId = new SessionId(sessionId);
    }

    public SessionId SessionId { get; }

    public static AdminSession CreateNew() => new(Guid.NewGuid());

    public static AdminSession CreateFromDb(Guid sessionId) => new(sessionId);
}