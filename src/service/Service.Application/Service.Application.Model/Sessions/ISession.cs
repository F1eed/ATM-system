using System.Text.Json.Serialization;

namespace Service.Application.Model.Sessions;

[JsonDerivedType(typeof(AdminSession), "AdminSession")]
[JsonDerivedType(typeof(UserSession), "UserSession")]
public interface ISession
{
    Guid SessionId { get; }
}