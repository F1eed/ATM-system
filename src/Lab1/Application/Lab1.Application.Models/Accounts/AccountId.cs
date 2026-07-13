namespace Lab1.Application.Models.Accounts;

public readonly record struct AccountId(long Value)
{
    public static readonly AccountId Default = new(default);
}