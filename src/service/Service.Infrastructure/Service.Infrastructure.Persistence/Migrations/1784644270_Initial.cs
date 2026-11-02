using FluentMigrator;

namespace Service.Infrastructure.Persistence.Migrations;

[Migration(1784644270, "Initial")]
public class Initial : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TABLE sessions (
              session_id uuid not null PRIMARY KEY,
              session_data jsonb not null
            );
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE sessions;
        ");
    }
}