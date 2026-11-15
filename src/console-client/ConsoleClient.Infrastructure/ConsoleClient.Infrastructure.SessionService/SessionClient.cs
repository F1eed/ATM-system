using ConsoleClient.Application.Abstractions.Session;
using ConsoleClient.Application.Abstractions.Session.Operations;
using ConsoleClient.Application.Model;
using ConsoleClient.Infrastructure.SessionService.Models.CreateAdmin;
using ConsoleClient.Infrastructure.SessionService.Models.CreateUser;
using Refit;

namespace ConsoleClient.Infrastructure.SessionService;

public class SessionClient : ISessionClient
{
    private readonly IRefitSessionClient _client;

    public SessionClient(IRefitSessionClient client)
    {
        _client = client;
    }

    public async Task<ServiceCreateAdminSession.Result> CreateAdminSessionAsync(
        ServiceCreateAdminSession.Request request,
        CancellationToken cancellationToken)
    {
        var httpRequest = new CreateAdminRequest(request.Password);

        IApiResponse<CreateAdminResponse> response = await _client.CreateAdminAsync(
            httpRequest,
            cancellationToken);

        // я не уверен норм ли поле такое брать и не обрабатывать IsSuccessful
        if (response.IsSuccessfulWithContent is false)
        {
            return new ServiceCreateAdminSession.Result.Failure(response.ReasonPhrase ?? string.Empty);
        }

        return new ServiceCreateAdminSession.Result.Success(
            new Session(response.Content.Id));
    }

    public async Task<ServiceCreateUserSession.Result> CreateUserSessionAsync(
        ServiceCreateUserSession.Request request,
        CancellationToken cancellationToken)
    {
        var httpRequest = new CreateUserRequest(
            request.CardNumber,
            request.PinCode);

        IApiResponse<CreateUserResponse> response = await _client.CreateUserAsync(
            httpRequest,
            cancellationToken);

        if (response.IsSuccessfulWithContent is false)
            return new ServiceCreateUserSession.Result.Failure(response.ReasonPhrase ?? string.Empty);

        return new ServiceCreateUserSession.Result.Success(
            new Session(response.Content.Id));
    }
}