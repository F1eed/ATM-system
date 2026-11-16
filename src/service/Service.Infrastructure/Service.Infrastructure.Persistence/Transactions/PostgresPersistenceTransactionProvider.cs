using Service.Application.Abstractions.Persistence.Transactions;
using System.Transactions;

namespace Service.Infrastructure.Persistence.Transactions;

public class PostgresPersistenceTransactionProvider : IPersistenceTransactionProvider
{
    public ValueTask<IPersistenceTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken)
    {
        var transaction = new PostgresPersistenceTransaction(isolationLevel);
        return ValueTask.FromResult<IPersistenceTransaction>(transaction);
    }
}