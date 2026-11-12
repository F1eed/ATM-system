using Service.Application.Abstractions.Persistence;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Abstractions.Persistence.Transactions;
using Service.Application.Contracts.Account;
using Service.Application.Contracts.Account.Models;
using Service.Application.Contracts.Account.Operations;
using Service.Application.Contracts.Operation.Models;
using Service.Application.Mapping;
using Service.Application.Model.Accounts;
using Service.Application.Model.Operations;
using Service.Application.Model.Sessions;
using System.Transactions;

namespace Service.Application.Services;

internal class AccountService : IAccountService
{
    private readonly IPersistenceContext _context;
    private readonly IPersistenceTransactionProvider _transactionProvider;

    public AccountService(IPersistenceContext context, IPersistenceTransactionProvider transactionProvider)
    {
        _context = context;
        _transactionProvider = transactionProvider;
    }

    public async Task<CreateAccount.Response> CreateAccountAsync(
        CreateAccount.Request request,
        CancellationToken cancellationToken)
    {
        var builder = SessionQuery.Build(builder => builder.WithId(request.SessionId));

        ISession? session = await _context.Sessions
            .QueryAsync(builder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (session is not AdminSession)
        {
            return new CreateAccount.Response.Failure("Permissions denied");
        }

        var list = request.Accounts.Select(x => x.MapToDomain()).ToList();

        List<AccountDto> dtos = await _context.Accounts
            .AddOrUpdateAsync(list, cancellationToken)
            .Select(x => x.MapToDto())
            .ToListAsync(cancellationToken);

        return new CreateAccount.Response.Success(dtos);
    }

    public async Task<CheckBalance.Response> CheckBalanceAsync(
        CheckBalance.Request request,
        CancellationToken cancellationToken)
    {
        var sessionBuilder = SessionQuery.Build(builder => builder.WithId(request.SessionId));

        ISession? session = await _context.Sessions
            .QueryAsync(sessionBuilder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (session is not UserSession userSession)
        {
            return new CheckBalance.Response.NotUserSession();
        }

        var accountBuilder = AccountQuery.Build(builder => builder.WithId(userSession.AccountId));
        Account? account = await _context.Accounts
            .QueryAsync(accountBuilder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return new CheckBalance.Response.Failure("Account is not found");
        }

        var operation = new CheckBalanceOperation(
            new OccuredAt(DateTimeOffset.UtcNow),
            OperationId.Default,
            account.AccountId,
            account.Balance);

        await _context.Operations.AddOrUpdateAsync([operation], cancellationToken);

        return new CheckBalance.Response.Success(account.Balance.Value);
    }

    public async Task<Deposit.Response> DepositAsync(
        Deposit.Request request,
        CancellationToken cancellationToken)
    {
        var sessionBuilder = SessionQuery.Build(builder => builder.WithId(request.SessionId));

        await using IPersistenceTransaction transaction = await _transactionProvider
            .BeginTransactionAsync(
                IsolationLevel.ReadCommitted,
                cancellationToken);

        ISession? session = await _context.Sessions
            .QueryAsync(sessionBuilder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (session is not UserSession userSession)
        {
            return new Deposit.Response.NotUserSession();
        }

        var accountBuilder = AccountQuery.Build(builder => builder.WithId(userSession.AccountId));
        Account? account = await _context.Accounts
            .QueryAsync(accountBuilder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return new Deposit.Response.Failure($"{nameof(Account)} is not found");
        }

        if (account.TryDeposit(request.Amount) is false)
        {
            return new Deposit.Response.Failure("Amount is incorrected");
        }

        var operation = new DepositOperation(
            new OccuredAt(DateTimeOffset.UtcNow),
            OperationId.Default,
            account.AccountId,
            new Money(request.Amount),
            account.Balance);

        await _context.Operations.AddOrUpdateAsync([operation], cancellationToken);

        AccountDto dto = await _context.Accounts
            .AddOrUpdateAsync([account], cancellationToken)
            .Select(x => x.MapToDto())
            .FirstAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new Deposit.Response.Success(dto);
    }

    public async Task<Withdraw.Response> WithdrawAsync(
        Withdraw.Request request,
        CancellationToken cancellationToken)
    {
        var sessionBuilder = SessionQuery.Build(builder => builder.WithId(request.SessionId));

        await using IPersistenceTransaction transaction = await _transactionProvider
            .BeginTransactionAsync(
                IsolationLevel.ReadCommitted,
                cancellationToken);

        ISession? session = await _context.Sessions
            .QueryAsync(sessionBuilder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (session is not UserSession userSession)
        {
            return new Withdraw.Response.NotUserSession();
        }

        var accountBuilder = AccountQuery.Build(builder => builder.WithId(userSession.AccountId));

        Account? account = await _context.Accounts
            .QueryAsync(accountBuilder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (account is null)
        {
            return new Withdraw.Response.Failure("Account is not found");
        }

        if (account.TryWithdraw(request.Amount) is false)
        {
            return new Withdraw.Response.Failure("Insufficient funds");
        }

        var operation = new WithdrawOperation(
            new OccuredAt(DateTimeOffset.UtcNow),
            OperationId.Default,
            account.AccountId,
            new Money(request.Amount),
            account.Balance);

        await _context.Operations
            .AddOrUpdateAsync([operation], cancellationToken);

        AccountDto dto = await _context.Accounts
            .AddOrUpdateAsync([account], cancellationToken)
            .Select(x => x.MapToDto())
            .FirstAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new Withdraw.Response.Success(dto);
    }

    public async Task<CheckHistory.Response> CheckHistoryAsync(
        CheckHistory.Request request,
        CancellationToken cancellationToken)
    {
        var sessionBuilder = SessionQuery.Build(builder => builder.WithId(request.SessionId));

        ISession? session = await _context.Sessions
            .QueryAsync(sessionBuilder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken);

        if (session is not UserSession userSession)
        {
            return new CheckHistory.Response.NotUserSession();
        }

        var operationQuery = OperationQuery.Build(builder => builder
            .WithAccountId(userSession.AccountId)
            .WithKeyCursor(request.PageToken?.Key)
            .WithPageSize(request.PageSize));

        IAsyncEnumerable<IOperation> operations = _context.Operations
            .QueryAsync(operationQuery, cancellationToken);

        List<BaseOperationDto> operationList = [];

        await foreach (IOperation operation in operations)
        {
            operationList.Add(operation.MapToDto());
        }

        CheckHistory.PageToken? responsePageToken = operationList.Count < request.PageSize
            ? null
            : new CheckHistory.PageToken(operationList[^1].OperationId);

        return new CheckHistory.Response.Success(operationList, responsePageToken);
    }
}