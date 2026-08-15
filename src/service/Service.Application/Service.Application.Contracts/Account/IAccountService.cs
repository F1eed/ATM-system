using Service.Application.Contracts.Account.Operations;

namespace Service.Application.Contracts.Account;

public interface IAccountService
{
    Task<CreateAccount.Response> CreateAccountAsync(
        CreateAccount.Request request,
        CancellationToken cancellationToken);

    Task<CheckBalance.Response> CheckBalanceAsync(
        CheckBalance.Request request,
        CancellationToken cancellationToken);

    Task<Deposit.Response> DepositAsync(
        Deposit.Request request,
        CancellationToken cancellationToken);

    Task<Withdraw.Response> WithdrawAsync(
        Withdraw.Request request,
        CancellationToken cancellationToken);

    Task<CheckHistory.Response> CheckHistoryAsync(
        CheckHistory.Request request,
        CancellationToken cancellationToken);
}