using Lab1.Application.Abstractions.Persistence;
using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Contracts.Account;
using Lab1.Application.Contracts.Account.Operations;
using Lab1.Application.Contracts.Operation.Models;
using Lab1.Application.Mapping;
using Lab1.Application.Models.Accounts;
using Lab1.Application.Models.Operations;
using Lab1.Application.Models.Sessions;

namespace Lab1.Application.Services;

internal class AccountService : IAccountService
{
    private readonly IPersistenceContext _context;

    public AccountService(IPersistenceContext context)
    {
        _context = context;
    }

    public CreateAccount.Response CreateAccount(CreateAccount.Request request)
    {
        var builder = SessionQuery.Build(builder => builder.WithId(request.SessionId));
        ISession? session = _context.Sessions
            .Query(builder)
            .SingleOrDefault();

        if (session is null)
        {
            return new CreateAccount.Response.Failure("Session is not found");
        }

        if (session is UserSession)
        {
            return new CreateAccount.Response.Failure("Permissions denied");
        }

        var account = new Account(
            AccountId.Default,
            request.Number,
            request.PinCode);

        account = _context.Accounts.Add(account);

        return new CreateAccount.Response.Success(account.MapToDto());
    }

    public CheckBalance.Response CheckBalance(CheckBalance.Request request)
    {
        var sessionBuilder = SessionQuery.Build(builder => builder.WithId(request.SessionId));
        ISession? session = _context.Sessions
            .Query(sessionBuilder)
            .SingleOrDefault();

        if (session is not UserSession userSession)
        {
            return new CheckBalance.Response.NotUserSession();
        }

        var accountBuilder = AccountQuery.Build(builder => builder.WithId(userSession.AccountId));
        Account? account = _context.Accounts
            .Query(accountBuilder)
            .SingleOrDefault();

        if (account is null)
        {
            return new CheckBalance.Response.Failure("Account is not found");
        }

        var operation = new CheckBalanceOperation(
            OperationId.Default,
            account.AccountId,
            account.Balance);

        _context.Operations.Add(operation);
        return new CheckBalance.Response.Success(account.Balance);
    }

    public Deposit.Response Deposit(Deposit.Request request)
    {
        var sessionBuilder = SessionQuery.Build(builder => builder.WithId(request.SessionId));
        ISession? session = _context.Sessions
            .Query(sessionBuilder)
            .SingleOrDefault();

        if (session is not UserSession userSession)
        {
            return new Deposit.Response.NotUserSession();
        }

        var accountBuilder = AccountQuery.Build(builder => builder.WithId(userSession.AccountId));
        Account? account = _context.Accounts
            .Query(accountBuilder)
            .SingleOrDefault();

        if (account is null)
        {
            return new Deposit.Response.Failure("Account is not found");
        }

        if (account.TryDeposit(request.Amount) is false)
        {
            return new Deposit.Response.Failure("Amount is incorrected");
        }

        var operation = new DepositOperation(
            OperationId.Default,
            account.AccountId,
            account.Balance);

        _context.Operations.Add(operation);
        return new Deposit.Response.Success(account.MapToDto());
    }

    public Withdraw.Response Withdraw(Withdraw.Request request)
    {
        var sessionBuilder = SessionQuery.Build(builder => builder.WithId(request.SessionId));
        ISession? session = _context.Sessions
            .Query(sessionBuilder)
            .SingleOrDefault();

        if (session is not UserSession userSession)
        {
            return new Withdraw.Response.NotUserSession();
        }

        var accountBuilder = AccountQuery.Build(builder => builder.WithId(userSession.AccountId));
        Account? account = _context.Accounts
            .Query(accountBuilder)
            .SingleOrDefault();

        if (account is null)
        {
            return new Withdraw.Response.Failure("Account is not found");
        }

        if (account.TryWithdraw(request.Amount) is false)
        {
            return new Withdraw.Response.Failure("Amount is incorrected");
        }

        var operation = new WithdrawOperation(
            OperationId.Default,
            account.AccountId,
            account.Balance);

        _context.Operations.Add(operation);
        return new Withdraw.Response.Success(account.MapToDto());
    }

    public CheckHistory.Response CheckHistory(CheckHistory.Request request)
    {
        var sessionBuilder = SessionQuery.Build(builder => builder.WithId(request.SessionId));
        ISession? session = _context.Sessions
            .Query(sessionBuilder)
            .SingleOrDefault();

        if (session is not UserSession userSession)
        {
            return new CheckHistory.Response.NotUserSession();
        }

        var operationBuilder = OperationQuery.Build(builder => builder.WithId(userSession.AccountId));
        IEnumerable<OperationBase> operations = _context.Operations
            .Query(operationBuilder);

        using IEnumerator<OperationBase> operationsEnumerator = operations.GetEnumerator();
        if (operationsEnumerator.MoveNext() is false)
        {
            return new CheckHistory.Response.Success([]);
        }

        List<OperationDto> operationList = [];

        while (operationsEnumerator.MoveNext())
        {
            operationList.Add(operationsEnumerator.Current.MapToDto());
        }

        return new CheckHistory.Response.Success(operationList);
    }
}