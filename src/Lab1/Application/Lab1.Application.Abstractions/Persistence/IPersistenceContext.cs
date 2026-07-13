using Lab1.Application.Abstractions.Persistence.Repositories;

namespace Lab1.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IAccountRepository Accounts { get; }

    IOperationRepository Operations { get; }

    ISessionRepository Sessions { get; }
}