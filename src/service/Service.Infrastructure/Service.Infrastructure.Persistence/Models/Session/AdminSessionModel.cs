namespace Service.Infrastructure.Persistence.Models.Session;

public record AdminSessionModel(Guid SessionId) : SessionModel(SessionId);
