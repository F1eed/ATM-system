using System.ComponentModel.DataAnnotations;

namespace Service.Infrastructure.Persistence.Options;

public class PostgresConnectionOptions
{
    [MinLength(1)]
    public required string Host { get; init; } = string.Empty;

    [Range(1, 65535)]
    public required int Port { get; init; } = 5432;

    [MinLength(1)]
    public required string Database { get; init; } = "PGUSER";

    [MinLength(1)]
    public required string Username { get; init; } = "PGUSER";

    [MinLength(1)]
    public required string Password { get; init; } = "PGPASSWORD";

    public string ToConnectionString()
    {
        return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
    }
}