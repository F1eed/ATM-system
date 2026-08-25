using ConsoleClient.Application.Abstractions.Account;
using ConsoleClient.Application.Abstractions.Account.Operations;
using ConsoleClient.Infrastructure.AccountService.Models;
using Refit;

namespace ConsoleClient.Infrastructure.AccountService;

public class AccountClient : IAccountClient
{
    private readonly IRefitAccountClient _accountClient;

    public AccountClient(IRefitAccountClient accountClient)
    {
        _accountClient = accountClient;
    }

    public async Task<CreateAccount.Result> CreateAccount(
        CreateAccount.Request request,
        CancellationToken cancellationToken)
    {
        var httpRequest = new CreateAccountRequest(
            request.CardNumber,
            request.PinCode);

        IApiResponse response = await _accountClient.CreateAccountAsync(
            httpRequest,
            cancellationToken);

        if (response.IsSuccessful is false)
        {
            return new CreateAccount.Result.Failure(
                response.ReasonPhrase ?? string.Empty);
        }

        return new CreateAccount.Result.Success();
    }
}