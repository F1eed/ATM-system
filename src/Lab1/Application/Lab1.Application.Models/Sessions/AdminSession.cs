namespace Lab1.Application.Models.Sessions;

public class AdminSession : ISession
{
    public AdminSession()
    {
        SessionId = Guid.NewGuid();
    }

    public Guid SessionId { get; }
}