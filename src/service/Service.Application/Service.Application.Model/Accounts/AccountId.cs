namespace Service.Application.Model.Accounts;

public readonly record struct AccountId(long Value)
{
    // TODO: конструктор с проверкой, что больше 0
    // тк в базе негодяй может поставить какашку и у тебя тут сломается
    public static readonly AccountId Default = new(default);
}