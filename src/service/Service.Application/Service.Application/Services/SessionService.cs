using Itmo.Dev.Platform.Persistence.Abstractions.Transactions;
using Microsoft.Extensions.Configuration;
using Service.Application.Abstractions.Persistence;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Contracts.Session;
using Service.Application.Contracts.Session.Operations;
using Service.Application.Mapping;
using Service.Application.Model.Accounts;
using Service.Application.Model.Sessions;
using System.Data;

namespace Service.Application.Services;

internal class SessionService : ISessionService
{
    private readonly IPersistenceContext _context;
    private readonly IPersistenceTransactionProvider _transactionProvider;

    // Надо ли всю конфигурацию тут хранить, когда по сути только
    // поле нужно одно и можно его строкой записать и через DI прокидывать
    private readonly IConfiguration _configuration;

    public SessionService(
        IPersistenceContext context,
        IPersistenceTransactionProvider transactionProvider,
        IConfiguration configuration)
    {
        _context = context;
        _transactionProvider = transactionProvider;
        _configuration = configuration;
    }

    public async Task<CreateAdminSession.Response> CreateAdminSessionAsync(
        CreateAdminSession.Request request,
        CancellationToken cancellationToken)
    {
        if (request.Password != _configuration["Password"])
        {
            return new CreateAdminSession.Response.Failure("Incorrect Password");
        }

        await using IPersistenceTransaction transaction = await _transactionProvider.BeginTransactionAsync(
                IsolationLevel.ReadCommitted,
                cancellationToken);

        var adminSession = new AdminSession();

        await _context.Sessions.AddOrUpdateAsync(
            adminSession,
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new CreateAdminSession.Response.Success(adminSession.MapToDto());
    }

    public async Task<CreateUserSession.Response> CreateUserSessionAsync(
        CreateUserSession.Request request,
        CancellationToken cancellationToken)
    {
        await using IPersistenceTransaction transaction = await _transactionProvider.BeginTransactionAsync(
            IsolationLevel.ReadCommitted,
            cancellationToken);

        AccountQuery builder = AccountQuery.Build(builder => builder
            .WithNumber(request.Number));

        Account? account = await _context.Accounts
            .QueryAsync(builder, cancellationToken)
            .SingleOrDefaultAsync(cancellationToken: cancellationToken);

        if (account is null)
        {
            return new CreateUserSession.Response.Failure("Account is not found");
        }

        if (account.PinCode != request.PinCode)
        {
            return new CreateUserSession.Response.Failure("Invalid pin");
        }

        var session = new UserSession(account.AccountId);

        await _context.Sessions.AddOrUpdateAsync(
            session,
            cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        return new CreateUserSession.Response.Success(session.MapToDto());
    }
}