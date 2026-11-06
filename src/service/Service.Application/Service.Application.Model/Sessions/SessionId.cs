namespace Service.Application.Model.Sessions;

public readonly record struct SessionId
{
    public SessionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }
}