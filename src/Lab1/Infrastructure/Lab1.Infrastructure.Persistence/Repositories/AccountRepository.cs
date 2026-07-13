using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Abstractions.Persistence.Repositories;
using Lab1.Application.Models.Accounts;

namespace Lab1.Infrastructure.Persistence.Repositories;

internal sealed class AccountRepository : IAccountRepository
{
    private readonly Dictionary<AccountId, Account> _values = [];

    public Account Add(Account account)
    {
        account = new Account(
            new AccountId(_values.Count + 1),
            account.Number,
            account.PinCode);

        _values.Add(account.AccountId, account);

        return account;
    }

    public IEnumerable<Account> Query(AccountQuery query)
    {
        return _values.Values
            .Where(x => query.Ids is [] || query.Ids.Contains(x.AccountId))
            .Where(x => query.Number is "" || query.Number == x.Number);
    }
}