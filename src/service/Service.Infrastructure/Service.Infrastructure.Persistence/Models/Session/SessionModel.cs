using System.Text.Json.Serialization;

namespace Service.Infrastructure.Persistence.Models.Session;

[JsonDerivedType(typeof(AdminSessionModel), "admin_session")]
[JsonDerivedType(typeof(UserSessionModel), "user_session")]
public abstract record SessionModel(Guid SessionId);
