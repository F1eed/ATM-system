namespace Service.Infrastructure.Persistence.Models.Session;

public record UserSessionModel(Guid SessionId, long AccountId) : SessionModel(SessionId);