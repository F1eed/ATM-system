using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Application.Model.Accounts;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Service.Infrastructure.Persistence.Repositories;

internal sealed class AccountRepository : IAccountRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public AccountRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task AddOrUpdateAsync(
        IReadOnlyCollection<Account> accounts,
        CancellationToken cancellationToken)
    {
        const string sql =
        """
        INSERT INTO accounts (card_number, pin_code, balance)
        SELECT source.card_number, source.pin_code, source.balance
        FROM unnest(:card_numbers, :pin_codes, :balances) AS source(card_number, pin_code, balance)
        ON CONFLICT ON CONSTRAINT unique_card_number DO UPDATE
        SET pin_code = excluded.pin_code,
            balance = excluded.balance;
        """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("card_numbers", accounts.Select(x => x.CardNumber))
            .AddParameter("pin_codes", accounts.Select(x => x.PinCode))
            .AddParameter("balances", accounts.Select(x => x.Balance));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<Account> QueryAsync(
        AccountQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql =
        """
        SELECT account_id,
              card_number,
              pin_code,
              balance
        FROM accounts
        WHERE (length(:number) = 0  or card_number = :number) AND (:id = 0 or :id = account_id);
        """; // Не особо уверен, что тут правильно сравнивать id с 0, тк при создании пользовательской сессии туда всегда идет 0

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("number", query.CardNumber)
            .AddParameter("id", query.Id.Value);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return new Account(
                new AccountId(reader.GetInt64("account_id")),
                reader.GetString("card_number"),
                reader.GetString("pin_code"),
                reader.GetDecimal("balance"));
        }
    }
}