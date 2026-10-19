using ConsoleClient.Application.Abstractions.Account;
using ConsoleClient.Application.Abstractions.Account.Operations;
using ConsoleClient.Application.Abstractions.Session;
using ConsoleClient.Application.Abstractions.Session.Operations;
using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using ConsoleClient.Application.Mapping;
using System.Diagnostics;

namespace ConsoleClient.Application;

public class BankService : IBankService
{
    private readonly ISessionClient _sessionClient;
    private readonly IAccountClient _accountClient;

    public BankService(ISessionClient sessionClient, IAccountClient accountClient)
    {
        _sessionClient = sessionClient;
        _accountClient = accountClient;
    }

    public async Task<CreateUserSession.Result> CreateUserSessionAsync(
        CreateUserSession.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new ServiceCreateUserSession.Request(
                request.CardNumber,
                request.PinCode);

        ServiceCreateUserSession.Result clientResponse = await _sessionClient.CreateUserSessionAsync(
            clientRequest,
            cancellationToken);

        if (clientResponse is ServiceCreateUserSession.Result.Success success)
        {
            return new CreateUserSession.Result.Success(success.Session.MapToDto());
        }

        if (clientResponse is ServiceCreateUserSession.Result.Failure failure)
        {
            return new CreateUserSession.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    public async Task<CreateAdminSession.Result> CreateAdminSessionAsync(
        CreateAdminSession.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new ServiceCreateAdminSession.Request(request.Password);

        ServiceCreateAdminSession.Result clientResult = await _sessionClient.CreateAdminSessionAsync(
            clientRequest,
            cancellationToken);

        if (clientResult is ServiceCreateAdminSession.Result.Success success)
        {
            return new CreateAdminSession.Result.Success(success.Session.MapToDto());
        }

        if (clientResult is ServiceCreateAdminSession.Result.Failure failure)
        {
            return new CreateAdminSession.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    public async Task<Deposit.Result> DepositAsync(
        Deposit.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new ServiceDeposit.Request(
                request.SessionId,
                request.Amount);

        ServiceDeposit.Result clientResult = await _accountClient.DepositAsync(
                clientRequest,
                cancellationToken);

        if (clientResult is ServiceDeposit.Result.Success success)
        {
            return new Deposit.Result.Success(success.Account.MapToDto());
        }

        if (clientResult is ServiceDeposit.Result.Failure failure)
        {
            return new Deposit.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    public async Task<Withdraw.Result> WithdrawAsync(
        Withdraw.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new ServiceWithdraw.Request(
                request.SessionId,
                request.Amount);

        ServiceWithdraw.Result clientResult = await _accountClient.WithdrawAsync(
                clientRequest,
                cancellationToken);

        if (clientResult is ServiceWithdraw.Result.Success success)
        {
            return new Withdraw.Result.Success(success.Account.MapToDto());
        }

        if (clientResult is ServiceWithdraw.Result.Failure failure)
        {
            return new Withdraw.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    public async Task<CheckBalance.Result> CheckBalanceAsync(
        CheckBalance.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new ServiceCheckBalance.Request(request.SessionId);

        ServiceCheckBalance.Result clientResponse = await _accountClient.CheckBalanceAsync(
                clientRequest,
                cancellationToken);

        if (clientResponse is ServiceCheckBalance.Result.Failure failure)
        {
            return new CheckBalance.Result.Failure(failure.ErrorMessage);
        }

        if (clientResponse is ServiceCheckBalance.Result.Success success)
        {
            return new CheckBalance.Result.Success(success.Balance);
        }

        throw new UnreachableException();
    }

    public async Task<CheckHistory.Result> CheckHistoryAsync(
        CheckHistory.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new ServiceCheckHistory.Request(request.SessionId);

        ServiceCheckHistory.Result clientResponse = await _accountClient.CheckHistoryAsync(
                clientRequest,
                cancellationToken);

        if (clientResponse is ServiceCheckHistory.Result.Failure failure)
        {
            return new CheckHistory.Result.Failure(failure.ErrorMessage);
        }

        if (clientResponse is ServiceCheckHistory.Result.Success success)
        {
            return new CheckHistory.Result.Success([
                .. success.OperationEntries.Select(entry => entry.MapToDto())
            ]);
        }

        throw new UnreachableException();
    }

    public async Task<CreateAccount.Result> CreateAccountAsync(
        CreateAccount.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new ServiceCreateAccount.Request(
                request.SessionId,
                request.Entries
                .Select(entry => new ServiceCreateAccount.AccountEntry(
                    entry.CardNumber,
                    entry.PinCode))
                .ToList());

        ServiceCreateAccount.Result clientResponse = await _accountClient.CreateAccountAsync(
                clientRequest,
                cancellationToken);

        if (clientResponse is ServiceCreateAccount.Result.Failure failure)
        {
            return new CreateAccount.Result.Failure(failure.ErrorMessage);
        }

        if (clientResponse is ServiceCreateAccount.Result.Success success)
        {
            return new CreateAccount.Result.Success(success.CardNumbers);
        }

        throw new UnreachableException();
    }
}
