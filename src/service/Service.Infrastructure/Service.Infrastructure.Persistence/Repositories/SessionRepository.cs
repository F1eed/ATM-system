using Itmo.Dev.Platform.Persistence.Abstractions.Commands;
using Itmo.Dev.Platform.Persistence.Abstractions.Connections;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Application.Model.Sessions;
using Service.Infrastructure.Persistence.Mapping.Session;
using Service.Infrastructure.Persistence.Models.Session;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace Service.Infrastructure.Persistence.Repositories;

internal sealed class SessionRepository : ISessionRepository
{
    private readonly IPersistenceConnectionProvider _connectionProvider;

    public SessionRepository(IPersistenceConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task AddOrUpdateAsync(
        ISession session,
        CancellationToken cancellationToken)
    {
        const string sql =
            """
            INSERT INTO sessions (session_id, session_data) 
            VALUES (:id, :data::jsonb)
            ON CONFLICT ON CONSTRAINT sessions_pkey DO UPDATE
              SET session_data = excluded.session_data
            """;

        await using IPersistenceConnection connection = await _connectionProvider
            .GetConnectionAsync(cancellationToken);

        SessionModel dbModel = session.ToDbModel();

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("data", JsonSerializer.Serialize(dbModel))
            .AddParameter("id", session.SessionId);

        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    public async IAsyncEnumerable<ISession> QueryAsync(
        SessionQuery query,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        const string sql =
        """
        SELECT session_id, session_data
        FROM sessions
        WHERE :session_ids IS NULL
           OR session_id = ANY(:session_ids);
        """;

        await using IPersistenceConnection connection = await _connectionProvider
            .GetConnectionAsync(cancellationToken);

        await using IPersistenceCommand command = connection.CreateCommand(sql)
            .AddParameter("session_ids", query.Ids);

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            SessionModel dbModel =
                JsonSerializer.Deserialize<SessionModel>(
                    await reader.GetFieldValueAsync<string>(
                        "session_data",
                        cancellationToken: cancellationToken))
                ?? throw new InvalidOperationException("Undefined type of session data");

            yield return dbModel.ToDomain();
        }
    }
}
