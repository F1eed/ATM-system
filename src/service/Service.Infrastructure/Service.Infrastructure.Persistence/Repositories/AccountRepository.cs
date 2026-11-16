using Npgsql;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Application.Model.Accounts;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Service.Infrastructure.Persistence.Repositories;

internal sealed class AccountRepository : IAccountRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public AccountRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async IAsyncEnumerable<Account> AddOrUpdateAsync(
        IReadOnlyCollection<Account> accounts,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql =
            """
            INSERT INTO accounts (card_number, pin_code, balance)
            SELECT source.card_number, source.pin_code, source.balance
            FROM unnest(:card_numbers, :pin_codes, :balances) AS source(card_number, pin_code, balance)
            ON CONFLICT ON CONSTRAINT accounts_card_number_key DO UPDATE
            SET pin_code = excluded.pin_code,
                balance = excluded.balance
            RETURNING account_id, card_number, pin_code, balance;
            """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(
            new NpgsqlParameter("card_numbers", accounts.Select(x => x.CardNumber).ToList()));
        command.Parameters.Add(
            new NpgsqlParameter("pin_codes", accounts.Select(x => x.PinCode).ToList()));
        command.Parameters.Add(
            new NpgsqlParameter("balances", accounts.Select(x => x.Balance.Value).ToList()));

        DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return Account.CreateFromDb(
                new AccountId(reader.GetInt64("account_id")),
                reader.GetString("card_number"),
                reader.GetString("pin_code"),
                reader.GetDecimal("balance"));
        }
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
            WHERE (cardinality(:numbers) = 0  or card_number = ANY(:numbers))
              AND (cardinality(:ids) = 0 or account_id = ANY(:ids));
        """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("numbers", query.CardNumbers));
        command.Parameters.Add(new NpgsqlParameter("ids", query.Ids.Select(x => x.Value).ToList()));

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return Account.CreateFromDb(
                new AccountId(reader.GetInt64("account_id")),
                reader.GetString("card_number"),
                reader.GetString("pin_code"),
                reader.GetDecimal("balance"));
        }
    }
}