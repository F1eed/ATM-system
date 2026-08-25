using ConsoleClient.Application.Abstractions.Account.Operations;

namespace ConsoleClient.Application.Abstractions.Account;

public interface IAccountClient
{
    Task<CreateAccount.Result> CreateAccount(
        CreateAccount.Request request,
        CancellationToken cancellationToken);
}