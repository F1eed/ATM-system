using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Application.Model.Accounts;
using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping;
using Service.Infrastructure.Persistence.Models.Operation;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace Service.Infrastructure.Persistence.Repositories;

internal sealed class OperationRepository : IOperationRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public OperationRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task AddOrUpdateAsync(
        IReadOnlyCollection<OperationBase> operations,
        CancellationToken cancellationToken)
    {
        const string sql =
        """
        INSERT INTO operations (account_id, balance, type)
        SELECT source.account_id, source.balance, source.type::operation_type
        FROM unnest (:account_ids, :balances, :types) AS SOURCE(account_id, balance, type)
        ON CONFLICT ON CONSTRAINT operations_pkey DO UPDATE
        SET account_id = excluded.account_id,
           balance = excluded.balance,
           type = excluded.type
        """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        OperationDao[] operationDao = operations
            .Select(operation => operation.MapToDao())
            .ToArray();

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("account_ids", operationDao.Select(dao => dao.AccountId))
            .AddParameter("balances", operationDao.Select(dao => dao.Balance))
            .AddParameter("types", operationDao.Select(dao => dao.Type));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<OperationBase> QueryAsync(
        OperationQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql = """
                           SELECT operation_id,
                                  account_id,
                                  balance,
                                  type
                           FROM operations
                           WHERE :account_id = account_id
                           """;

        await using IPersistenceConnection connection = await _connectionProvider.GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("account_id", query.AccountId.Value);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            // странно(или нет?)
            switch (reader.GetString("type"))
            {
                case "withdraw":
                    yield return new WithdrawOperation(
                        new OperationId(reader.GetInt64("operation_id")),
                        new AccountId(reader.GetInt64("account_id")),
                        reader.GetDecimal("balance"));
                    break;
                case "deposit":
                    yield return new DepositOperation(
                        new OperationId(reader.GetInt64("operation_id")),
                        new AccountId(reader.GetInt64("account_id")),
                        reader.GetDecimal("balance"));
                    break;
                case "check_balance":
                    yield return new CheckBalanceOperation(
                        new OperationId(reader.GetInt64("operation_id")),
                        new AccountId(reader.GetInt64("account_id")),
                        reader.GetDecimal("balance"));
                    break;
            }
        }
    }
}