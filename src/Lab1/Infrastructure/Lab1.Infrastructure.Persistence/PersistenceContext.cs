using Lab1.Application.Abstractions.Persistence;
using Lab1.Application.Abstractions.Persistence.Repositories;

namespace Lab1.Infrastructure.Persistence;

internal class PersistenceContext : IPersistenceContext
{
    public PersistenceContext(IAccountRepository accounts, IOperationRepository operations, ISessionRepository sessions)
    {
        Accounts = accounts;
        Operations = operations;
        Sessions = sessions;
    }

    public IAccountRepository Accounts { get; }

    public IOperationRepository Operations { get; }

    public ISessionRepository Sessions { get; }
}