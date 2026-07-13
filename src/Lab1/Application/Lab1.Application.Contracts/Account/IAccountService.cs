using Lab1.Application.Contracts.Account.Operations;

namespace Lab1.Application.Contracts.Account;

public interface IAccountService
{
    CreateAccount.Response CreateAccount(CreateAccount.Request request);

    CheckBalance.Response CheckBalance(CheckBalance.Request request);

    Deposit.Response Deposit(Deposit.Request request);

    Withdraw.Response Withdraw(Withdraw.Request request);

    CheckHistory.Response CheckHistory(CheckHistory.Request request);
}