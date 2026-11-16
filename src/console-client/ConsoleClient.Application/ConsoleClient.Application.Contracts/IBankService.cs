using ConsoleClient.Application.Contracts.Operations;

namespace ConsoleClient.Application.Contracts;

public interface IBankService
{
    Task<CreateUserSession.Result> CreateUserSessionAsync(
        CreateUserSession.Request request,
        CancellationToken cancellationToken);

    Task<CreateAdminSession.Result> CreateAdminSessionAsync(
        CreateAdminSession.Request request,
        CancellationToken cancellationToken);

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