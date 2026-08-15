using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1784644270, "Initial")]
public class Initial : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        CREATE TABLE sessions (
          session_id uuid not null PRIMARY KEY,
          session_data jsonb not null
        );
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        DROP TABLE sessions;
        """;
}