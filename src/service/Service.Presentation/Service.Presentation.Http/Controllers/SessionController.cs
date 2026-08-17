using Microsoft.AspNetCore.Mvc;
using Service.Application.Contracts.Session;
using Service.Application.Contracts.Session.Operations;
using Service.Presentation.Http.Models;
using System.Diagnostics;

namespace Service.Presentation.Http.Controllers;

[ApiController]
[Route("api/session/create")]
public sealed class SessionController : ControllerBase
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost("admin")]
    public async Task<ActionResult<CreateAdminResponse>> CreateAdminAsync(
        [FromBody] CreateAdminRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var request = new CreateAdminSession.Request(httpRequest.Password);

        CreateAdminSession.Response response = await _sessionService.CreateAdminSessionAsync(
            request,
            cancellationToken);

        return response switch
        {
            CreateAdminSession.Response.Success success => Ok(new CreateAdminResponse(success.Session.Id)),
            CreateAdminSession.Response.Failure failure => BadRequest(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("user")]
    public async Task<ActionResult<CreateUserResponse>> CreateUserAsync(
        [FromBody] CreateUserRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var request = new CreateUserSession.Request(httpRequest.Entries
            .Select(entry => new CreateUserSession.UserEntry(entry.Number, entry.PinCode))
            .ToList());

        CreateUserSession.Response response = await _sessionService.CreateUserSessionAsync(
            request,
            cancellationToken);

        if (response is CreateUserSession.Response.Success success)
        {
            var entries = success.Sessions
                .Select(entry => new CreateUserResponse.UserSessionEntry(entry.Id))
                .ToList();
            return Ok(
                new CreateUserResponse(entries));
        }
        else if (response is CreateUserSession.Response.Failure failure)
        {
            return Unauthorized(failure.Error);
        }
        else
        {
            throw new UnreachableException();
        }

        // return response switch
        // {
        //     CreateUserSession.Response.Success success => Ok(success.Sessions),
        //     CreateUserSession.Response.Failure failure => Unauthorized(failure.Error),
        //     _ => throw new UnreachableException(),
        // }; Убрал это потому что теперь не дтошки возвращаю а тип который хранит в себе энтри, хз норм нет?
    }
}