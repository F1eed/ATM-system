namespace Service.Application.Model.Accounts;

public class Account
{
    private const decimal ZeroBalance = 0m;

    private Account(
        AccountId accountId,
        string cardNumber,
        string pinCode,
        decimal balance)
    {
        AccountId = accountId;
        CardNumber = cardNumber;
        PinCode = pinCode;
        Balance = new Money(balance);
    }

    public static Account CreateNew(
        AccountId accountId,
        string cardNumber,
        string pinCode)
    {
        return new Account(accountId, cardNumber, pinCode, ZeroBalance);
    }

    public static Account CreateFromDb(
        AccountId accountId,
        string cardNumber,
        string pinCode,
        decimal balance)
    {
        return new Account(accountId, cardNumber, pinCode, balance);
    }

    public AccountId AccountId { get; }

    public string CardNumber { get; }

    public string PinCode { get; }

    public Money Balance { get; private set; }

    public bool TryWithdraw(decimal amount)
    {
        if (amount <= 0 || amount > Balance.Value)
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