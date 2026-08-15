namespace Service.Application.Model.Sessions;

public class AdminSession : ISession
{
    public AdminSession()
    {
        SessionId = Guid.NewGuid();
    }

    public Guid SessionId { get; }
}