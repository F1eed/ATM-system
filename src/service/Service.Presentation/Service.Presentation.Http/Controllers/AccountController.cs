using Microsoft.AspNetCore.Mvc;
using Service.Application.Contracts.Account;
using Service.Application.Contracts.Account.Operations;
using Service.Presentation.Http.Models;
using System.Diagnostics;

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
        var list = httpRequest.Entries3
            .Select(entry =>
                new CreateAccount.Account(entry.CardNumber, entry.PinCode))
            .ToList();

        var request = new CreateAccount.Request(httpRequest.SessionId, list);

        CreateAccount.Response response = await _accountService.CreateAccountAsync(
            request,
            cancellationToken);

        return response switch
        {
            CreateAccount.Response.Success success => Ok(new CreateAccountResponse(
                success.Accounts
                    .Select(dto => new AccountEntry(
                        dto.CardNumber,
                        dto.Balance))
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
                new DepositResponse(new AccountEntry(
                    success.Account.CardNumber,
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
                new AccountEntry(
                    success.Account.CardNumber,
                    success.Account.Balance))),
            Withdraw.Response.NotUserSession => Unauthorized(),
            Withdraw.Response.Failure failure => BadRequest(failure.Message),
            _ => throw new UnreachableException(),
        };
    }

    // чёт странно что апи с аккаунтом связано, но требует sessionId(отмена, там по условию так в 5ой лабе)
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
        CancellationToken cancellationToken)
    {
        var request = new CheckHistory.Request(sessionId);

        CheckHistory.Response response = await _accountService.CheckHistoryAsync(
            request,
            cancellationToken);

        return response switch
        {
            CheckHistory.Response.Success success => Ok(
                new CheckHistoryResponse(success.Operations)),
            CheckHistory.Response.NotUserSession => Unauthorized(),
            CheckHistory.Response.Failure failure => BadRequest(failure.Message),
            _ => throw new UnreachableException(),
        };
    }
}