using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Model.Operations;

namespace Service.Application.Abstractions.Persistence.Repositories;

public interface IOperationRepository
{
    Task AddOrUpdateAsync(
        IReadOnlyCollection<OperationBase> operations,
        CancellationToken cancellationToken);

    IAsyncEnumerable<OperationBase> QueryAsync(
        OperationQuery query,
        CancellationToken cancellationToken);
}