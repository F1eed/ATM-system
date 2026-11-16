namespace Service.Application.Model.Operations;

public readonly record struct OccuredAt
{
    public OccuredAt(DateTimeOffset value)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, DateTimeOffset.UtcNow);
        Value = value;
    }

    public DateTimeOffset Value { get; }
}