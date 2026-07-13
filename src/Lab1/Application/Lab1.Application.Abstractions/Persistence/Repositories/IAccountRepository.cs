using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Models.Accounts;

namespace Lab1.Application.Abstractions.Persistence.Repositories;

public interface IAccountRepository
{
    Account Add(Account account);

    IEnumerable<Account> Query(AccountQuery query);
}