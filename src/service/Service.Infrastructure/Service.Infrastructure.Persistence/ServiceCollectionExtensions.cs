using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.Application.Abstractions.Persistence;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Application.Abstractions.Persistence.Transactions;
using Service.Infrastructure.Persistence.Factories;
using Service.Infrastructure.Persistence.Options;
using Service.Infrastructure.Persistence.Repositories;
using Service.Infrastructure.Persistence.Transactions;

namespace Service.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection collection)
    {
        public IServiceCollection AddInfrastructurePersistence(IConfiguration configuration)
        {
            collection.AddPostgresDatabase(configuration);
            collection.AddSingleton(OperationChainFactory.Create());
            collection.AddRepositories();

            return collection;
        }

        private void AddPostgresDatabase(IConfiguration configuration)
        {
            IConfigurationSection postgresSection = configuration
                .GetSection("Infrastructure:Persistence:Postgres");

            collection
                .AddOptions<PostgresConnectionOptions>()
                .BindConfiguration("Infrastructure:Persistence:Postgres")
                .ValidateDataAnnotations()
                .ValidateOnStart();

            PostgresConnectionOptions postgresOptions = postgresSection.Get<PostgresConnectionOptions>()
                                                        ?? throw new InvalidOperationException(
                                                            "Postgres section is not found");

            string connectionString = postgresOptions.ToConnectionString();

            collection.AddNpgsqlDataSource(connectionString);

            collection
                .AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder => runnerBuilder
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .WithMigrationsIn(typeof(IAssemblyMarker).Assembly))
                .AddLogging(logger => logger.AddFluentMigratorConsole());

            collection.AddScoped<IPersistenceTransactionProvider, PostgresPersistenceTransactionProvider>();
            collection.AddScoped<IPersistenceContext, PersistenceContext>();
        }

        private void AddRepositories()
        {
            collection.AddScoped<IAccountRepository, AccountRepository>();
            collection.AddScoped<IOperationRepository, OperationRepository>();
            collection.AddScoped<ISessionRepository, SessionRepository>();
        }
    }
}