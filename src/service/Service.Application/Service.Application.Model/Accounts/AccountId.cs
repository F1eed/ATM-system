namespace Service.Application.Model.Accounts;

public readonly record struct AccountId
{
    public static readonly AccountId Default = new(0);

    public AccountId(long value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    public long Value { get; }
}