using Lab1.Application.Abstractions.Persistence;
using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Contracts.Session;
using Lab1.Application.Contracts.Session.Operations;
using Lab1.Application.Mapping;
using Lab1.Application.Models.Accounts;
using Lab1.Application.Models.Sessions;
using Microsoft.Extensions.Configuration;

namespace Lab1.Application.Services;

internal class SessionService : ISessionService
{
    private readonly IPersistenceContext _context;
    private readonly IConfiguration _configuration;

    public SessionService(IPersistenceContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public CreateAdminSession.Response CreateAdminSession(CreateAdminSession.Request request)
    {
        if (request.Password != _configuration["AdminPassword"])
        {
            return new CreateAdminSession.Response.Failure("IncorrectPassword");
        }

        var adminSession = new AdminSession();

        _context.Sessions.Add(adminSession);
        return new CreateAdminSession.Response.Success(adminSession.MapToDto());
    }

    public CreateUserSession.Response CreateUserSession(CreateUserSession.Request request)
    {
        var builder = AccountQuery.Build(builder => builder.WithNumber(request.Number));

        Account? account = _context.Accounts
            .Query(builder).SingleOrDefault();

        if (account is null)
        {
            return new CreateUserSession.Response.Failure("Account is not found");
        }

        if (account.PinCode != request.PinCode)
        {
            return new CreateUserSession.Response.Failure("Invalid pin");
        }

        var userSession = new UserSession(account.AccountId);

        _context.Sessions.Add(userSession);
        return new CreateUserSession.Response.Success(userSession.MapToDto());
    }
}