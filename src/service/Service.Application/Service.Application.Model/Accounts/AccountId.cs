namespace Service.Application.Model.Accounts;

public readonly record struct AccountId(long Value)
{
    public static readonly AccountId Default = new(default);
}