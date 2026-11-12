using ConsoleClient.Application.Contracts.Models;
using ConsoleClient.Application.Model;

namespace ConsoleClient.Application.Mapping;

public static class SessionMappingExtensions
{
    public static SessionDto MapToDto(this Session session)
    {
        return new SessionDto(session.Id);
    }
}