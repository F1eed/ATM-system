namespace Service.Application.Model.Accounts;

public class Account
{
    // TODO: можно сделать конструктор приватным и сделать 2 публичный статик метода CreateNew и CreateFromDb
    // у которых уже будут разные аргументы
    public Account(
        AccountId accountId,
        string cardNumber,
        string pinCode,
        decimal balance = 0) // пришлось сюда добавить баланс(поговорить бы об этом)
    {
        AccountId = accountId;
        CardNumber = cardNumber;
        PinCode = pinCode;
        Balance = balance;
    }

    public AccountId AccountId { get; }

    public string CardNumber { get; }

    public string PinCode { get; }

    // TODO: vo для денег
    public decimal Balance { get; private set; }

    public bool TryWithdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance)
        {
            return false;
        }

        Balance -= amount;

        return true;
    }

    public bool TryDeposit(decimal amount)
    {
        if (amount < 0)
        {
            return false;
        }

        Balance += amount;
        return true;
    }
}