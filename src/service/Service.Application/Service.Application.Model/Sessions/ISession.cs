using System.Text.Json.Serialization;

namespace Service.Application.Model.Sessions;

// TODO: тут такого писать не надо. Надо сделать модельки на уровне Service.Infrastructure.Persistence
// (абстрактный рекорд и его наследники и эти аттрибуты делать у абстарктного рекорда
[JsonDerivedType(typeof(AdminSession), "AdminSession")]
[JsonDerivedType(typeof(UserSession), "UserSession")]
public interface ISession
{
    Guid SessionId { get; }
}