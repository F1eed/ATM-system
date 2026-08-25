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

    public BankService(ISessionClient sessionClient)
    {
        _sessionClient = sessionClient;
    }

    public async Task<CreateUserSession.Result> CreateUserSessionAsync(
        CreateUserSession.Request request,
        CancellationToken cancellationToken)
    {
        var clientRequest = new Abstractions.Session
            .Operations.CreateUserSession.Request(
                request.Number,
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
            CreateAdminSession.Result clientResponse = await _sessionClient.CreateAdminSessionAsync(
            clientRequest,
            cancellationToken);

        if (clientResponse is Abstractions.Session.Operations.CreateAdminSession.Result.Success success)
        {
            return new CreateAdminSession.Result.Success(success.Session.MapToDto());
        }

        if (clientResponse is Abstractions.Session.Operations.CreateAdminSession.Result.Failure failure)
        {
            return new CreateAdminSession.Result.Failure(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }
}
