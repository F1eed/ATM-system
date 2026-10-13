namespace Service.Application.Model.Accounts;

public readonly record struct Money
{
    public Money(decimal value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);
        Value = value;
    }

    public decimal Value { get; }

    public static Money operator +(Money left, decimal right) => new Money(left.Value + right);

    public static Money operator +(Money left, Money right) => new Money(left.Value + right.Value);

    public static Money operator -(Money left, decimal right) => new Money(left.Value - right);

    public static Money operator -(Money left, Money right) => new Money(left.Value - right.Value);
}