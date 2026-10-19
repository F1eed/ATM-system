using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1784909608, "Operations")]
public class AddOperations : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        CREATE TABLE "operations" (
          "operation_id" bigserial not null PRIMARY KEY,
          "occured_at" timestamptz not null default now(),
          "account_id" bigserial not null,
          "operation_data" jsonb not null
        );
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        DROP TABLE operations;
        """;
}