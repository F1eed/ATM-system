using System.Transactions;

namespace Service.Application.Abstractions.Persistence.Transactions;

public interface IPersistenceTransactionProvider
{
    ValueTask<IPersistenceTransaction> BeginTransactionAsync(
        IsolationLevel isolationLevel,
        CancellationToken cancellationToken);
}