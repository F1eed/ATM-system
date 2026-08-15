using Service.Application.Contracts.Session.Models;
using Service.Application.Model.Sessions;

namespace Service.Application.Mapping;

public static class SessionMappingExtensions
{
    public static SessionDto MapToDto(this ISession session)
        => new SessionDto(session.SessionId);
}