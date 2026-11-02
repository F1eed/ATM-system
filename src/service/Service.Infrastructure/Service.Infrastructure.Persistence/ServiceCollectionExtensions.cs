using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Application.Abstractions.Persistence;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Infrastructure.Persistence.Factories;
using Service.Infrastructure.Persistence.Options;
using Service.Infrastructure.Persistence.Repositories;

namespace Service.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(
        this IServiceCollection collection,
        IConfiguration configuration)
    {
        IConfigurationSection postgresSection = configuration
            .GetSection("Infrastructure:Persistence:Postgres");

        collection
            .AddOptions<PostgresConnectionOptions>()
            .BindConfiguration("Infrastructure:Persistence:Postgres")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        PostgresConnectionOptions postgresOptions = postgresSection.Get<PostgresConnectionOptions>()
            ?? throw new InvalidOperationException("Postgres section is not found");

        string connectionString = postgresOptions.ToConnectionString();

        collection.AddNpgsqlDataSource(connectionString);

        collection
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .WithMigrationsIn(typeof(IAssemblyMarker).Assembly))
            .AddLogging(logger => logger.AddFluentMigratorConsole());

        collection.AddSingleton(OperationChainFactory.Create());

        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddScoped<IAccountRepository, AccountRepository>();
        collection.AddScoped<IOperationRepository, OperationRepository>();
        collection.AddScoped<ISessionRepository, SessionRepository>();

        return collection;
    }
}