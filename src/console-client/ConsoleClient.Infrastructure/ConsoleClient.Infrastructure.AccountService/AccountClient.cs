using ConsoleClient.Application.Abstractions.Account;
using ConsoleClient.Application.Abstractions.Account.Operations;
using ConsoleClient.Application.Model;
using ConsoleClient.Infrastructure.AccountService.Models;
using Refit;

namespace ConsoleClient.Infrastructure.AccountService;

public class AccountClient : IAccountClient
{
    private readonly IRefitAccountClient _refitAccountClient;

    public AccountClient(IRefitAccountClient refitAccountClient)
    {
        _refitAccountClient = refitAccountClient;
    }

    public async Task<CreateAccount.Result> CreateAccountAsync(
        CreateAccount.Request request,
        CancellationToken cancellationToken)
    {
        var httpRequest = new CreateAccountRequest(
            request.SessionId,
            request.Entries
                .Select(entry => new CreateAccountRequest.AccountEntry(
                    entry.CardNumber,
                    entry.PinCode))
                .ToList());

        IApiResponse<CreateAccountResponse> response = await _refitAccountClient.CreateAccountAsync(
            httpRequest,
            cancellationToken);

        if (response.IsSuccessfulWithContent is true)
        {
            return new CreateAccount.Result.Success(response.Content.CardNumbers);
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new CreateAccount.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        // Норм что "?"
        return new CreateAccount.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }

    public async Task<Deposit.Result> DepositAsync(
        Deposit.Request request,
        CancellationToken cancellationToken)
    {
        // Чет хуйни тут наворотил(не нрав, что тащу в депозите sessionId, хотя это в route ручки у меня)
        var httpRequest = new DepositRequest(
            request.SessionId,
            request.Amount);

        // Есть вопросик
        IApiResponse<DepositResponse> response = await _refitAccountClient.DepositMoneyAsync(
            httpRequest.SessionId,
            httpRequest,
            cancellationToken);

        if (response.IsSuccessfulWithContent is true)
        {
            // А норм ли тут создавать доменную модельку клиента?
            return new Deposit.Result.Success(new Account(
                response.Content.Entry.CardNumber,
                response.Content.Entry.Balance));
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new Deposit.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        // Норм что "?"
        return new Deposit.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }

    public async Task<Withdraw.Result> WithdrawAsync(
        Withdraw.Request request,
        CancellationToken cancellationToken)
    {
        var httpRequest = new WithdrawRequest(
            request.SessionId,
            request.Amount);

        IApiResponse<WithdrawResponse> response = await _refitAccountClient.WithdrawMoneyAsync(
            httpRequest.SessionId,
            httpRequest,
            cancellationToken);

        if (response.IsSuccessfulWithContent is true)
        {
            // А норм ли тут создавать доменную модельку клиента?
            return new Withdraw.Result.Success(new Account(
                response.Content.Entry.CardNumber,
                response.Content.Entry.Balance));
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new Withdraw.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        // Норм что "?"
        return new Withdraw.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }

    public async Task<CheckBalance.Result> CheckBalanceAsync(
        CheckBalance.Request request,
        CancellationToken cancellationToken)
    {
        IApiResponse<CheckBalanceResponse> response = await _refitAccountClient.CheckBalanceAsync(
            request.SessionId,
            cancellationToken);

        if (response.IsSuccessfulWithContent is true)
        {
            return new CheckBalance.Result.Success(response.Content.Balance);
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new CheckBalance.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        // Норм что "?"
        return new CheckBalance.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }

    public async Task<CheckHistory.Result> CheckHistoryAsync(
        CheckHistory.Request request,
        CancellationToken cancellationToken)
    {
        IApiResponse<CheckHistoryResponse> response = await _refitAccountClient.CheckHistoryAsync(
            request.SessionId,
            cancellationToken);

        if (response.IsSuccessfulWithContent is true)
        {
            return new CheckHistory.Result.Success(
                response.Content.Operations.Select(entry => new Operation(
                    entry.OperationId,
                    entry.AccountId,
                    entry.Type,
                    entry.Balance))
                .ToList());
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new CheckHistory.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        // Норм что "?"
        return new CheckHistory.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }
}
