using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping.Operation;
using Service.Infrastructure.Persistence.Mapping.Operation.Results;
using Service.Infrastructure.Persistence.Models.Operation;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Service.Infrastructure.Persistence.Repositories;

internal sealed class OperationRepository : IOperationRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;
    private readonly IOperationLink _link;

    public OperationRepository(IPersistenceConnectionProvider connectionProvider, IOperationLink link)
    {
        _connectionProvider = connectionProvider;
        _link = link;
    }

    public async Task AddOrUpdateAsync(
        IReadOnlyCollection<IOperation> operations,
        CancellationToken cancellationToken)
    {
        const string sql =
        """
        INSERT INTO operations (occured_at, account_id, operation_data)
        SELECT source.occured_at, source.account_id, source.data::jsonb
        FROM unnest (:occured_ats, :account_ids, :datas) AS SOURCE(occured_at, account_id, data)
        """;

        await using IPersistenceConnection connection = await _connectionProvider
            .GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("occured_ats", operations.Select(operation => operation.OccuredAt.Value))
            .AddParameter("account_ids", operations.Select(operation => operation.AccountId.Value))
            .AddParameter("datas", operations.Select(MapOperationToJson));

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<IOperation> QueryAsync(
        OperationQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql =
        """
        SELECT operation_id, occured_at, account_id, operation_data
        FROM operations
        WHERE :account_id = account_id
        ORDER BY occured_at DESC
        """;

        await using IPersistenceConnection connection = await _connectionProvider
            .GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("account_id", query.AccountId.Value);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            yield return MapReaderToOperation(reader);
        }
    }

    private IOperation MapReaderToOperation(DbDataReader reader)
    {
        OperationModel model =
            JsonSerializer.Deserialize<OperationModel>(reader.GetFieldValue<string>("operation_data"))
            ?? throw new InvalidOperationException("model is not found");

        DateTimeOffset occuredAt = reader.GetDateTime("occured_at");
        long operationId = reader.GetInt64("operation_id");
        long accountId = reader.GetInt64("account_id");

        return _link.MapToDomain(occuredAt, operationId, accountId, model) is MappingToDomainResult.Success success
            ? success.Operation
            : throw new InvalidOperationException("mapper is not found");
    }

    private string MapOperationToJson(IOperation operation)
    {
        return _link.MapToModel(operation) is MappingToModelResult.Success success
            ? JsonSerializer.Serialize(success.Model)
            : throw new InvalidOperationException("mapper is not found");
    }
}