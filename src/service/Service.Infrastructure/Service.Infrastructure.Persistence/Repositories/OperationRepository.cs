using Npgsql;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Application.Model.Operations;
using Service.Infrastructure.Persistence.Mapping.Operation;
using Service.Infrastructure.Persistence.Mapping.Operation.Results;
using Service.Infrastructure.Persistence.Models.Operation;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Service.Infrastructure.Persistence.Repositories;

internal sealed class OperationRepository : IOperationRepository
{
    private readonly NpgsqlDataSource _dataSource;
    private readonly IOperationLink _link;

    public OperationRepository(IOperationLink link, NpgsqlDataSource dataSource)
    {
        _link = link;
        _dataSource = dataSource;
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

        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("occured_ats", operations
            .Select(operation => operation.OccuredAt.Value).ToList()));
        command.Parameters.Add(new NpgsqlParameter("account_ids", operations
            .Select(operation => operation.AccountId.Value).ToList()));
        command.Parameters.Add(new NpgsqlParameter("datas", operations.Select(MapOperationToJson).ToList()));

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

        await using NpgsqlConnection connection = await _dataSource
            .OpenConnectionAsync(cancellationToken);

        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.Add(new NpgsqlParameter("account_id", query.AccountId.Value));

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

        if (_link.MapToDomain(occuredAt, operationId, accountId, model)
            is MappingToDomainResult.Success success)
        {
            return success.Operation;
        }

        if (_link.MapToDomain(occuredAt, operationId, accountId, model) is MappingToDomainResult.Failure failure)
        {
            throw new InvalidOperationException(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }

    private string MapOperationToJson(IOperation operation)
    {
        if (_link.MapToModel(operation)
            is MappingToModelResult.Success success)
        {
            return JsonSerializer.Serialize(success.Model);
        }

        if (_link.MapToModel(operation) is MappingToModelResult.Failure failure)
        {
            throw new InvalidOperationException(failure.ErrorMessage);
        }

        throw new UnreachableException();
    }
}