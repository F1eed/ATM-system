using Microsoft.AspNetCore.Mvc;
using Service.Application.Contracts.Session;
using Service.Application.Contracts.Session.Models;
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
    public async Task<ActionResult<SessionDto>> CreateAdmin(
        [FromBody] CreateAdminRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var request = new CreateAdminSession.Request(httpRequest.Password);

        CreateAdminSession.Response response = await _sessionService.CreateAdminSessionAsync(
            request,
            cancellationToken);

        return response switch
        {
            CreateAdminSession.Response.Success success => Ok(success.Session.Id),
            CreateAdminSession.Response.Failure failure => BadRequest(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("user")]
    public async Task<ActionResult<SessionDto>> CreateUser(
        [FromBody] CreateUserRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var request = new CreateUserSession.Request(httpRequest.Entries
            .Select(entry => new CreateUserSession.UserEntry(entry.Number, entry.PinCode))
            .ToList());

        CreateUserSession.Response response = await _sessionService.CreateUserSessionAsync(
            request,
            cancellationToken);

        return response switch
        {
            CreateUserSession.Response.Success success => Ok(success.Sessions),
            CreateUserSession.Response.Failure failure => Unauthorized(failure.Error),
            _ => throw new UnreachableException(),
        };
    }
}