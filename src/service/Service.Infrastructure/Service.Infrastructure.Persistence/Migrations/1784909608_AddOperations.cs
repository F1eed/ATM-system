using FluentMigrator;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1784909608, "Operations")]
public class AddOperations : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE operations (
                operation_id bigserial not null PRIMARY KEY,
                occured_at timestamptz not null default now(),
                account_id bigserial not null,
                operation_data jsonb not null
            );
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE operations;
        ");
    }
}