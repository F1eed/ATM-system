using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1784909608, "Operations")]
public class AddOperations : SqlMigration
{
    // может появиться операция, которая хранит больше/меньше/другие данные
    // использовать идею как с сессиями - хранить что-то в жсоне
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        CREATE TYPE "operation_type" AS ENUM (
          'withdraw',
          'deposit',
          'check_balance'
        );
        
        CREATE TABLE "operations" (
          "operation_id" bigserial not null PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
          "account_id" bigserial not null,
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