using Service.Application.Abstractions.Persistence.Transactions;
using System.Transactions;

namespace Service.Infrastructure.Persistence.Transactions;

internal class PostgresPersistenceTransaction : IPersistenceTransaction
{
    private readonly TransactionScope _scope;

    private bool _disposed;

    public PostgresPersistenceTransaction(IsolationLevel isolationLevel)
    {
        _scope = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = isolationLevel },
            TransactionScopeAsyncFlowOption.Enabled);
    }

    public ValueTask DisposeAsync()
    {
        if (_disposed is false)
        {
            _scope.Dispose();
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask CommitAsync(CancellationToken cancellationToken)
    {
        _scope.Complete();

        _disposed = true;
        _scope.Dispose();

        return ValueTask.CompletedTask;
    }

    public ValueTask RollbackAsync(CancellationToken cancellationToken)
    {
        _disposed = true;
        _scope.Dispose();

        return ValueTask.CompletedTask;
    }
}