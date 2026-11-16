using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Model.Sessions;

namespace Service.Application.Abstractions.Persistence.Repositories;

public interface ISessionRepository
{
    Task AddOrUpdateAsync(
        ISession session,
        CancellationToken cancellationToken);

    IAsyncEnumerable<ISession> QueryAsync(
        SessionQuery query,
        CancellationToken cancellationToken);
}