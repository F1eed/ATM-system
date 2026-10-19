namespace Service.Application.Model.Sessions;

public class AdminSession : ISession
{
    private AdminSession(Guid sessionId)
    {
        SessionId = sessionId;
    }

    public Guid SessionId { get; }

    public static AdminSession CreateNew() => new(Guid.NewGuid());

    public static AdminSession CreateFromDb(Guid sessionId) => new(sessionId);
}