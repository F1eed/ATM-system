using FluentMigrator;
using Itmo.Dev.Platform.Persistence.Postgres.Migrations;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1784909321, "Accounts")]
public class AddAccounts : SqlMigration
{
    protected override string GetUpSql(IServiceProvider serviceProvider) =>
        """
        CREATE TABLE accounts (
          account_id bigserial not null PRIMARY KEY,
          card_number varchar(16) not null CHECK (length(card_number) = 16) UNIQUE,
          pin_code varchar(4) not null CHECK (length(pin_code) = 4),
          balance decimal
        );
        """;

    protected override string GetDownSql(IServiceProvider serviceProvider) =>
        """
        DROP TABLE accounts;
        """;
}