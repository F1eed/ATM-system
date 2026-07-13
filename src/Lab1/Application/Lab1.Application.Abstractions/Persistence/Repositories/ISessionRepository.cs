using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Models.Sessions;

namespace Lab1.Application.Abstractions.Persistence.Repositories;

public interface ISessionRepository
{
    void Add(ISession operation);

    IEnumerable<ISession> Query(SessionQuery query);
}