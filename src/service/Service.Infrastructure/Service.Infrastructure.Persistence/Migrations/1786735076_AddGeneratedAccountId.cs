using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1786735076, "Generate AccountId Value")]
public class AddGeneratedAccountId : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        ALTER TABLE accounts ALTER COLUMN account_id ADD GENERATED ALWAYS AS IDENTITY;    
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        ALTER TABLE accounts ALTER COLUMN account_id DROP IDENTITY;
        """;
}