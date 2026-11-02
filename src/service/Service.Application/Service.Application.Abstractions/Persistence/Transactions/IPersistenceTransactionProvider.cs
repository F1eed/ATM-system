namespace Service.Application.Abstractions.Persistence.Transactions;

public interface IPersistenceTransactionProvider
{
    ValueTask<IPersistenceTransaction> BeginTransactionAsync(
        System.Transactions.IsolationLevel isolationLevel,
        CancellationToken cancellationToken);
}