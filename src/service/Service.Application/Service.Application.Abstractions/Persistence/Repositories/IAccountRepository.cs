using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Model.Accounts;

namespace Service.Application.Abstractions.Persistence.Repositories;

public interface IAccountRepository
{
    Task AddOrUpdateAsync(
        IReadOnlyCollection<Account> accounts,
        CancellationToken cancellationToken);

    IAsyncEnumerable<Account> QueryAsync(
        AccountQuery query,
        CancellationToken cancellationToken);
}