using Microsoft.Extensions.Options;
using Service.Application.Abstractions.Persistence;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Contracts.Session;
using Service.Application.Contracts.Session.Operations;
using Service.Application.Mapping;
using Service.Application.Model.Accounts;
using Service.Application.Model.Sessions;
using Service.Application.Services.Options;

namespace Service.Application.Services;

internal class SessionService : ISessionService
{
    private readonly IPersistenceContext _context;
    private readonly SessionServiceOptions _options;

    public SessionService(
        IPersistenceContext context,
        IOptions<SessionServiceOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    public async Task<CreateAdminSession.Response> CreateAdminSessionAsync(
        CreateAdminSession.Request request,
        CancellationToken cancellationToken)
    {
        if (request.Password != _options.Password)
        {
            return new CreateAdminSession.Response.Failure("Incorrect Password");
        }

        var adminSession = AdminSession.CreateNew();

        await _context.Sessions.AddOrUpdateAsync(
            adminSession,
            cancellationToken);

        return new CreateAdminSession.Response.Success(adminSession.MapToDto());
    }

    public async Task<CreateUserSession.Response> CreateUserSessionAsync(
        CreateUserSession.Request request,
        CancellationToken cancellationToken)
    {
        AccountQuery builder = AccountQuery.Build(builder => builder
            .WithCardNumber(request.CardNumber));

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

        var session = UserSession.CreateNew(account.AccountId);

        await _context.Sessions.AddOrUpdateAsync(
            session,
            cancellationToken);

        return new CreateUserSession.Response.Success(session.MapToDto());
    }
}
