using System.ComponentModel.DataAnnotations;

namespace Service.Infrastructure.Persistence.Options;

public class PostgresConnectionOptions
{
    [MinLength(1)]
    public required string Host { get; init; } = string.Empty;

    [Range(1, 65535)]
    public required int Port { get; init; }

    [MinLength(1)]
    public required string Database { get; init; } = string.Empty;

    [MinLength(1)]
    public required string Username { get; init; } = string.Empty;

    [MinLength(1)]
    public required string Password { get; init; } = string.Empty;

    public string ToConnectionString()
    {
        return $"Host={Host};Port={Port};Database={Database};Username={Username};Password={Password}";
    }
}