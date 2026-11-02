using FluentMigrator;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1784909321, "Accounts")]
public class AddAccounts : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE accounts (
              account_id bigserial not null PRIMARY KEY,
              card_number varchar(16) not null CHECK (length(card_number) = 16) UNIQUE,
              pin_code varchar(4) not null CHECK (length(pin_code) = 4),
              balance decimal
            );
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE accounts
        ");
    }
}