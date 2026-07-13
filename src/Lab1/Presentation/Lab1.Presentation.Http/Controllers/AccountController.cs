using Lab1.Application.Contracts.Account;
using Lab1.Application.Contracts.Account.Models;
using Lab1.Application.Contracts.Account.Operations;
using Lab1.Application.Contracts.Operation.Models;
using Lab1.Presentation.Http.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab1.Presentation.Http.Controllers;

[ApiController]
[Route("api/account")]
public sealed class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("create")]
    public ActionResult<AccountDto> CreateAccount([FromBody] CreateAccountRequest httpRequest)
    {
        var request = new CreateAccount.Request(httpRequest.SessionId, httpRequest.Number, httpRequest.PinCode);

        CreateAccount.Response response = _accountService.CreateAccount(request);

        return response switch
        {
            CreateAccount.Response.Success success => Ok(success.Account.Number),
            CreateAccount.Response.Failure failure => Unauthorized(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("deposit")]
    public ActionResult<AccountDto> DepositMoney([FromBody] DepositRequest httpRequest)
    {
        var request = new Deposit.Request(httpRequest.SessionId, httpRequest.Amount);

        Deposit.Response response = _accountService.Deposit(request);

        return response switch
        {
            Deposit.Response.Success success => Ok($"Your balance: {success.Account.Balance}"),
            Deposit.Response.NotUserSession => Unauthorized(),
            Deposit.Response.Failure failure => BadRequest(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("withdraw")]
    public ActionResult<AccountDto> WithdrawMoney([FromBody] WithdrawRequest httpRequest)
    {
        var request = new Withdraw.Request(httpRequest.SessionId, httpRequest.Amount);

        Withdraw.Response response = _accountService.Withdraw(request);

        return response switch
        {
            Withdraw.Response.Success success => Ok($"Your remaining balance: {success.Account.Balance}"),
            Withdraw.Response.NotUserSession => Unauthorized(),
            Withdraw.Response.Failure failure => BadRequest(failure.Message),
            _ => throw new UnreachableException(),
        };
    }

    [HttpGet("balance/{sessionId}")]
    public ActionResult<AccountDto> CheckAccountBalance(Guid sessionId)
    {
        var request = new CheckBalance.Request(sessionId);

        CheckBalance.Response response = _accountService.CheckBalance(request);

        return response switch
        {
            CheckBalance.Response.Success success => Ok(success.Balance),
            CheckBalance.Response.NotUserSession => Unauthorized(),
            CheckBalance.Response.Failure failure => BadRequest(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpGet("history/{sessionId}")]
    public ActionResult<OperationDto> CheckAccountHistory(Guid sessionId)
    {
        var request = new CheckHistory.Request(sessionId);

        CheckHistory.Response response = _accountService.CheckHistory(request);

        return response switch
        {
            CheckHistory.Response.Success success => Ok(success.Operations),
            CheckHistory.Response.NotUserSession => Unauthorized(),
            CheckHistory.Response.Failure failure => BadRequest(failure.Message),
            _ => throw new UnreachableException(),
        };
    }
}