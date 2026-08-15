using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1786736475, "Add Unique Constraint in Card Number")]
public class AddUniqueOnCardNumber : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        ALTER TABLE accounts ADD CONSTRAINT unique_card_number UNIQUE (card_number);  
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        ALTER TABLE accounts DROP CONSTRAINT IF EXISTS unique_card_number;
        """;
}