using Lab1.Application.Contracts.Session.Models;
using Lab1.Application.Models.Sessions;

namespace Lab1.Application.Mapping;

public static class SessionMappingExtensions
{
    public static SessionDto MapToDto(this ISession session)
        => new SessionDto(session.SessionId);
}