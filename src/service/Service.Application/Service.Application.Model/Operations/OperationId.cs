namespace Service.Application.Model.Operations;

public readonly record struct OperationId
{
    public static readonly OperationId Default = new(0);

    public OperationId(long value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    public long Value { get; }
}