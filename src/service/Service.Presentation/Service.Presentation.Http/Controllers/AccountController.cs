using Microsoft.AspNetCore.Mvc;
using Service.Application.Contracts.Account;
using Service.Application.Contracts.Account.Operations;
using Service.Presentation.Http.Models;
using System.Diagnostics;
using System.Text.Json;

namespace Service.Presentation.Http.Controllers;

[ApiController]
[Route("api/account")]
public sealed class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost]
    public async Task<ActionResult<CreateAccountResponse>> CreateAccountAsync(
        [FromBody] CreateAccountRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var list = httpRequest.Entries
            .Select(entry =>
                new CreateAccount.AccountEntry(entry.CardNumber, entry.PinCode))
            .ToList();

        var request = new CreateAccount.Request(httpRequest.SessionId, list);

        CreateAccount.Response response = await _accountService.CreateAccountAsync(
            request,
            cancellationToken);

        return response switch
        {
            CreateAccount.Response.Success success => Ok(
                new CreateAccountResponse(
                    success.Accounts
                        .Select(dto => dto.CardNumber)
                        .ToList())),
            CreateAccount.Response.Failure failure => Unauthorized(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("{sessionId}/deposits")]
    public async Task<ActionResult<DepositResponse>> DepositMoneyAsync(
        [FromRoute] Guid sessionId,
        [FromBody] DepositRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var request = new Deposit.Request(sessionId, httpRequest.Amount);

        Deposit.Response response = await _accountService.DepositAsync(
            request,
            cancellationToken);

        return response switch
        {
            Deposit.Response.Success success => Ok(
                new DepositResponse(new DepositResponse.DepositAccountEntry(
                    success.Account.CardNumber,
                    httpRequest.Amount,
                    success.Account.Balance))),
            Deposit.Response.NotUserSession => Unauthorized(),
            Deposit.Response.Failure failure => BadRequest(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("{sessionId}/withdraws")]
    public async Task<ActionResult<WithdrawResponse>> WithdrawMoneyAsync(
        [FromRoute] Guid sessionId,
        [FromBody] WithdrawRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var request = new Withdraw.Request(sessionId, httpRequest.Amount);

        Withdraw.Response response = await _accountService.WithdrawAsync(
            request,
            cancellationToken);

        return response switch
        {
            Withdraw.Response.Success success => Ok(new WithdrawResponse(
                new WithdrawResponse.WithdrawAccountEntry(
                    success.Account.CardNumber,
                    httpRequest.Amount,
                    success.Account.Balance))),
            Withdraw.Response.NotUserSession => Unauthorized(),
            Withdraw.Response.Failure failure => BadRequest(failure.Message),
            _ => throw new UnreachableException(),
        };
    }

    [HttpGet("{sessionId}/balance")]
    public async Task<ActionResult<CheckBalanceResponse>> CheckAccountBalanceAsync(
        [FromRoute] Guid sessionId,
        CancellationToken cancellationToken)
    {
        var request = new CheckBalance.Request(sessionId);

        CheckBalance.Response response = await _accountService.CheckBalanceAsync(
            request,
            cancellationToken);

        return response switch
        {
            CheckBalance.Response.Success success => Ok(
                new CheckBalanceResponse(success.Balance)),
            CheckBalance.Response.NotUserSession => Unauthorized(),
            CheckBalance.Response.Failure failure => BadRequest(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpGet("{sessionId}/operations")]
    public async Task<ActionResult<CheckHistoryResponse>> CheckAccountHistoryAsync(
        Guid sessionId,
        [FromQuery] CheckHistoryRequest request,
        CancellationToken cancellationToken)
    {
        CheckHistory.PageToken? pageToken = request.PageToken is null
            ? null
            : new CheckHistory.PageToken(
                JsonSerializer.Deserialize<long>(request.PageToken));

        var applicationRequest = new CheckHistory.Request(
            sessionId,
            pageToken,
            request.PageSize);

        CheckHistory.Response applicationResponse = await _accountService.CheckHistoryAsync(
            applicationRequest,
            cancellationToken);

        if (applicationResponse is CheckHistory.Response.Failure failure)
        {
            return BadRequest(failure.Message);
        }

        if (applicationResponse is CheckHistory.Response.NotUserSession)
        {
            return Unauthorized();
        }

        if (applicationResponse is CheckHistory.Response.Success success)
        {
            string? responsePageToken = success.PageToken is null
                ? null
                : JsonSerializer.Serialize(success.PageToken.Value.Key);

            return Ok(new CheckHistoryResponse
            {
                Operations = success.Operations,
                PageToken = responsePageToken,
            });
        }

        throw new UnreachableException();
    }
}