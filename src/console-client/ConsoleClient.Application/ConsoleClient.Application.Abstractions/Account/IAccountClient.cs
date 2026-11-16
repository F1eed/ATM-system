using ConsoleClient.Application.Abstractions.Account.Operations;

namespace ConsoleClient.Application.Abstractions.Account;

public interface IAccountClient
{
    Task<ServiceCreateAccount.Result> CreateAccountAsync(
        ServiceCreateAccount.Request request,
        CancellationToken cancellationToken);

    Task<ServiceDeposit.Result> DepositAsync(
        ServiceDeposit.Request request,
        CancellationToken cancellationToken);

    Task<ServiceWithdraw.Result> WithdrawAsync(
        ServiceWithdraw.Request request,
        CancellationToken cancellationToken);

    Task<ServiceCheckBalance.Result> CheckBalanceAsync(
        ServiceCheckBalance.Request request,
        CancellationToken cancellationToken);

    Task<ServiceCheckHistory.Result> CheckHistoryAsync(
        ServiceCheckHistory.Request request,
        CancellationToken cancellationToken);
}