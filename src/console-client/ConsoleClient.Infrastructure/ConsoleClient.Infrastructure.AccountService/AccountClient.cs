using ConsoleClient.Application.Abstractions.Account;
using ConsoleClient.Application.Abstractions.Account.Operations;
using ConsoleClient.Application.Model;
using ConsoleClient.Application.Model.Operations;
using ConsoleClient.Infrastructure.AccountService.Mapping;
using ConsoleClient.Infrastructure.AccountService.Models;
using ConsoleClient.Infrastructure.AccountService.Options;
using Microsoft.Extensions.Options;
using Refit;

namespace ConsoleClient.Infrastructure.AccountService;

public class AccountClient : IAccountClient
{
    private readonly IOptionsMonitor<AccountClientOptions> _options;
    private readonly IRefitAccountClient _refitAccountClient;

    public AccountClient(
        IRefitAccountClient refitAccountClient,
        IOptionsMonitor<AccountClientOptions> options)
    {
        _refitAccountClient = refitAccountClient;
        _options = options;
    }

    public async Task<ServiceCreateAccount.Result> CreateAccountAsync(
        ServiceCreateAccount.Request request,
        CancellationToken cancellationToken)
    {
        var httpRequest = new CreateAccountRequest(
            request.SessionId,
            request.Entries
                .Select(entry =>
                    new CreateAccountRequest.AccountEntry(entry.CardNumber, entry.PinCode))
                .ToList());

        IApiResponse<CreateAccountResponse> response = await _refitAccountClient.CreateAccountAsync(
            httpRequest,
            cancellationToken);

        if (response.IsSuccessfulWithContent)
        {
            return new ServiceCreateAccount.Result.Success(response.Content.CardNumbers);
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new ServiceCreateAccount.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        // Норм что "?"
        return new ServiceCreateAccount.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }

    public async Task<ServiceDeposit.Result> DepositAsync(
        ServiceDeposit.Request request,
        CancellationToken cancellationToken)
    {
        var httpRequest = new DepositRequest(request.Amount);

        IApiResponse<DepositResponse> response = await _refitAccountClient.DepositMoneyAsync(
            request.SessionId,
            httpRequest,
            cancellationToken);

        if (response.IsSuccessfulWithContent)
        {
            // А норм ли тут создавать доменную модельку клиента? (Думаю норм учитывая что это инфра)
            return new ServiceDeposit.Result.Success(new Account(
                response.Content.Entry.CardNumber,
                response.Content.Entry.Balance));
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new ServiceDeposit.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        // Норм что "?"
        return new ServiceDeposit.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }

    public async Task<ServiceWithdraw.Result> WithdrawAsync(
        ServiceWithdraw.Request request,
        CancellationToken cancellationToken)
    {
        var httpRequest = new WithdrawRequest(
            request.Amount);

        IApiResponse<WithdrawResponse> response = await _refitAccountClient.WithdrawMoneyAsync(
            request.SessionId,
            httpRequest,
            cancellationToken);

        if (response.IsSuccessfulWithContent)
        {
            return new ServiceWithdraw.Result.Success(new Account(
                response.Content.Entry.CardNumber,
                response.Content.Entry.Balance));
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new ServiceWithdraw.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        return new ServiceWithdraw.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }

    public async Task<ServiceCheckBalance.Result> CheckBalanceAsync(
        ServiceCheckBalance.Request request,
        CancellationToken cancellationToken)
    {
        IApiResponse<CheckBalanceResponse> response = await _refitAccountClient.CheckBalanceAsync(
            request.SessionId,
            cancellationToken);

        if (response.IsSuccessfulWithContent)
        {
            return new ServiceCheckBalance.Result.Success(response.Content.Balance);
        }

        if (response.HasResponseError(out ApiException? apiException))
        {
            return new ServiceCheckBalance.Result.Failure(
                apiException.Content ?? string.Empty);
        }

        return new ServiceCheckBalance.Result.Failure(
            response.Error?.Message ?? string.Empty);
    }

    public async Task<ServiceCheckHistory.Result> CheckHistoryAsync(
        ServiceCheckHistory.Request request,
        CancellationToken cancellationToken)
    {
        var operations = new List<IOperation>(request.OperationsCount);

        int pageSize = _options.CurrentValue.PageSize;
        string? pageToken = null;

        do
        {
            int remaining = request.OperationsCount - operations.Count;
            int requestPageSize = Math.Min(remaining, pageSize);

            var clientRequest = new CheckHistoryRequest(pageToken, requestPageSize);

            IApiResponse<CheckHistoryResponse> response = await _refitAccountClient.CheckHistoryAsync(
                request.SessionId,
                clientRequest,
                cancellationToken);

            if (response.HasResponseError(out ApiException? apiException))
            {
                return new ServiceCheckHistory.Result.Failure(
                    apiException.Content ?? string.Empty);
            }

            if (response.IsSuccessfulWithContent is false)
            {
                return new ServiceCheckHistory.Result.Failure(
                    response.Error?.Message ?? string.Empty);
            }

            operations.AddRange(response.Content.Operations.Select(x => x.MapToModel()));

            pageToken = response.Content.PageToken;
        }
        while (pageToken is not null && operations.Count < request.OperationsCount);

        return new ServiceCheckHistory.Result.Success(operations);
    }
}