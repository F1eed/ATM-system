using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Model.Operations;

namespace Service.Application.Abstractions.Persistence.Repositories;

public interface IOperationRepository
{
    Task AddOrUpdateAsync(
        IReadOnlyCollection<IOperation> operations,
        CancellationToken cancellationToken);

    IAsyncEnumerable<IOperation> QueryAsync(
        OperationQuery query,
        CancellationToken cancellationToken);
}