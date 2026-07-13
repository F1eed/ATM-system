using Lab1.Application.Contracts.Session;
using Lab1.Application.Contracts.Session.Models;
using Lab1.Application.Contracts.Session.Operations;
using Lab1.Presentation.Http.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab1.Presentation.Http.Controllers;

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
    public ActionResult<SessionDto> CreateAdmin([FromBody] CreateAdminRequest httpRequest)
    {
        var request = new CreateAdminSession.Request(httpRequest.Password);

        CreateAdminSession.Response response = _sessionService.CreateAdminSession(request);
        return response switch
        {
            CreateAdminSession.Response.Success success => Ok(success.Session.Id),
            CreateAdminSession.Response.Failure failure => BadRequest(failure.Error),
            _ => throw new UnreachableException(),
        };
    }

    [HttpPost("user")]
    public ActionResult<SessionDto> CreateUser([FromBody] CreateUserRequest httpRequest)
    {
        var request = new CreateUserSession.Request(httpRequest.Number, httpRequest.PinCode);

        CreateUserSession.Response response = _sessionService.CreateUserSession(request);
        return response switch
        {
            CreateUserSession.Response.Success success => Ok(success.Session.Id),
            CreateUserSession.Response.Failure failure => Unauthorized(failure.Error),
            _ => throw new UnreachableException(),
        };
    }
}