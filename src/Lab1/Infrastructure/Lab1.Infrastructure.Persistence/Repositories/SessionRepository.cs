using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Abstractions.Persistence.Repositories;
using Lab1.Application.Models.Sessions;

namespace Lab1.Infrastructure.Persistence.Repositories;

internal sealed class SessionRepository : ISessionRepository
{
    private readonly Dictionary<Guid, ISession> _values = [];

    public void Add(ISession operation)
    {
        _values.Add(operation.SessionId, operation);
    }

    public IEnumerable<ISession> Query(SessionQuery query)
    {
        return _values.Values
            .Where(x => query.Ids is [] || query.Ids.Contains(x.SessionId));
    }
}