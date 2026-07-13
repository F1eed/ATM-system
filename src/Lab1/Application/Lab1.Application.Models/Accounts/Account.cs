namespace Lab1.Application.Models.Accounts;

public class Account
{
    public Account(AccountId accountId, string number, string pinCode)
    {
        AccountId = accountId;
        Number = number;
        PinCode = pinCode;
        Balance = 0;
    }

    public AccountId AccountId { get; }

    public string Number { get; }

    public string PinCode { get; }

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