using ConsoleClient.Application.Abstractions.Account;
using ConsoleClient.Application.Abstractions.Session;
using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using ConsoleClient.Application.Mapping;
using System.Diagnostics;

namespace ConsoleClient.Application;

// По ощущениям это будет ебануто большой сервис
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
        var clientRequest = new Abstractions.Session
            .Operations.CreateUserSession.Request(
                request.CardNumber,
                request.PinCode);

        // Выглядит как ужас
        Abstractions.Session.Operations.CreateUserSession
            .Result clientResponse = await _sessionClient.CreateUserSessionAsync(
            clientRequest,
            cancellationToken);

        // ебаный ужас реально
        if (clientResponse is Abstractions.Session.Operations.CreateUserSession.Result.Success success)
        {
            return new CreateUserSession.Result.Success(success.Session.MapToDto());
        }

        if (clientResponse is Abstractions.Session.Operations.CreateUserSession.Result.Failure failure)
        {
            return new CreateUserSession.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    public async Task<CreateAdminSession.Result> CreateAdminSessionAsync(
        CreateAdminSession.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new Abstractions.Session.Operations
            .CreateAdminSession.Request(request.Password);

        Abstractions.Session.Operations.
            CreateAdminSession.Result clientResult = await _sessionClient.CreateAdminSessionAsync(
            clientRequest,
            cancellationToken);

        if (clientResult is Abstractions.Session.Operations.CreateAdminSession.Result.Success success)
        {
            return new CreateAdminSession.Result.Success(success.Session.MapToDto());
        }

        if (clientResult is Abstractions.Session.Operations.CreateAdminSession.Result.Failure failure)
        {
            return new CreateAdminSession.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    public async Task<Deposit.Result> DepositAsync(
        Deposit.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new Abstractions.Account.Operations
            .Deposit.Request(
                request.SessionId,
                request.Amount);

        Abstractions.Account.Operations
            .Deposit.Result clientResult = await _accountClient.DepositAsync(
                clientRequest,
                cancellationToken);

        if (clientResult is Abstractions.Account.Operations.Deposit.Result.Success success)
        {
            return new Deposit.Result.Success(success.Account.MapToDto());
        }

        if (clientResult is Abstractions.Account.Operations.Deposit.Result.Failure failure)
        {
            return new Deposit.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    public async Task<Withdraw.Result> WithdrawAsync(
        Withdraw.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new Abstractions.Account.Operations
            .Withdraw.Request(
                request.SessionId,
                request.Amount);

        Abstractions.Account.Operations
            .Withdraw.Result clientResult = await _accountClient.WithdrawAsync(
                clientRequest,
                cancellationToken);

        if (clientResult is Abstractions.Account.Operations.Withdraw.Result.Success success)
        {
            return new Withdraw.Result.Success(success.Account.MapToDto());
        }

        if (clientResult is Abstractions.Account.Operations.Withdraw.Result.Failure failure)
        {
            return new Withdraw.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    public async Task<CheckBalance.Result> CheckBalanceAsync(
        CheckBalance.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new Abstractions.Account
            .Operations.CheckBalance.Request(request.SessionId);

        Abstractions.Account.Operations
            .CheckBalance.Result clientResponse = await _accountClient.CheckBalanceAsync(
                clientRequest,
                cancellationToken);

        if (clientResponse is Abstractions.Account.Operations.CheckBalance.Result.Failure failure)
        {
            return new CheckBalance.Result.Failure(failure.ErrorMessage);
        }

        if (clientResponse is Abstractions.Account.Operations.CheckBalance.Result.Success success)
        {
            return new CheckBalance.Result.Success(success.Balance);
        }

        throw new UnreachableException();
    }

    public async Task<CheckHistory.Result> CheckHistoryAsync(
        CheckHistory.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new Abstractions.Account
            .Operations.CheckHistory.Request(request.SessionId);

        Abstractions.Account
            .Operations.CheckHistory.Result clientResponse = await _accountClient.CheckHistoryAsync(
                clientRequest,
                cancellationToken);

        if (clientResponse is Abstractions.Account.Operations.CheckHistory.Result.Failure failure)
        {
            return new CheckHistory.Result.Failure(failure.ErrorMessage);
        }

        if (clientResponse is Abstractions.Account.Operations.CheckHistory.Result.Success success)
        {
            return new CheckHistory.Result.Success(
                success.OperationEntries.Select(
                        entry => entry.MapToDto())
                    .ToList());
        }

        throw new UnreachableException();
    }

    public async Task<CreateAccount.Result> CreateAccountAsync(
        CreateAccount.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new Abstractions.Account
            .Operations.CreateAccount.Request(
                request.SessionId,
                request.Entries
                .Select(entry => new Abstractions.Account.Operations.CreateAccount.AccountEntry(
                    entry.CardNumber,
                    entry.PinCode))
                .ToList());

        Abstractions.Account.Operations
            .CreateAccount.Result clientResponse = await _accountClient.CreateAccountAsync(
                clientRequest,
                cancellationToken);

        if (clientResponse is Abstractions.Account.Operations.CreateAccount.Result.Failure failure)
        {
            return new CreateAccount.Result.Failure(failure.ErrorMessage);
        }

        if (clientResponse is Abstractions.Account.Operations.CreateAccount.Result.Success success)
        {
            return new CreateAccount.Result.Success(success.CardNumbers);
        }

        throw new UnreachableException();
    }
}
