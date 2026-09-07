using ConsoleClient.Application.Abstractions.Account.Operations;

namespace ConsoleClient.Application.Abstractions.Account;

public interface IAccountClient
{
    Task<CreateAccount.Result> CreateAccountAsync(
        CreateAccount.Request request,
        CancellationToken cancellationToken);

    Task<Deposit.Result> DepositAsync(
        Deposit.Request request,
        CancellationToken cancellationToken);

    Task<Withdraw.Result> WithdrawAsync(
        Withdraw.Request request,
        CancellationToken cancellationToken);

    Task<CheckBalance.Result> CheckBalanceAsync(
        CheckBalance.Request request,
        CancellationToken cancellationToken);

    Task<CheckHistory.Result> CheckHistoryAsync(
        CheckHistory.Request request,
        CancellationToken cancellationToken);
}