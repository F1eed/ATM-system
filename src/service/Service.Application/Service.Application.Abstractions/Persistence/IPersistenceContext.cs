using Service.Application.Abstractions.Persistence.Repositories;

namespace Service.Application.Abstractions.Persistence;

public interface IPersistenceContext
{
    IAccountRepository Accounts { get; }

    IOperationRepository Operations { get; }

    ISessionRepository Sessions { get; }
}