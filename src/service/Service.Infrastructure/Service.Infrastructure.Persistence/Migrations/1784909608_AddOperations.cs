using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1784909608, "Operations")]
public class AddOperations : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        CREATE TYPE "operation_type" AS ENUM (
          'withdraw',
          'deposit',
          'check_balance'
        );
        
        CREATE TABLE "operations" (
          "operation_id" bigint not null PRIMARY KEY,
          "account_id" bigint not null,
          "balance" decimal not null,
          "type" operation_type not null
        );
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        DROP TABLE operations;
        DROP TYPE operation_type;
        """;
}