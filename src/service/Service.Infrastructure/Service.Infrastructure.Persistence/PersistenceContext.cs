using Service.Application.Abstractions.Persistence;
using Service.Application.Abstractions.Persistence.Repositories;

namespace Service.Infrastructure.Persistence;

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