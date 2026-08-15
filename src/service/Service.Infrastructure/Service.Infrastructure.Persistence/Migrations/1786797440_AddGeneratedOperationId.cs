using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1786797440, "Add Generated OperationId")]
public class AddGeneratedOperationId : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        ALTER TABLE operations ALTER operation_id ADD GENERATED ALWAYS AS IDENTITY;
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        ALTER TABLE operations ALTER operation_id DROP IDENTITY;
        """;
}