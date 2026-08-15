using Itmo.Dev.Platform.Persistence.Abstractions.Extensions;
using Itmo.Dev.Platform.Persistence.Postgres.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Service.Application.Abstractions.Persistence;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Infrastructure.Persistence.Repositories;

namespace Service.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection collection)
    {
        collection.AddPlatformPersistence(persistence => persistence
            .UsePostgres(postgres => postgres
                .WithConnectionOptions(builder => builder.BindConfiguration(
                    "Infrastructure:Persistence:Postgres"))
                .WithMigrationsFrom(typeof(IAssemblyMarker).Assembly)));

        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddScoped<IAccountRepository, AccountRepository>();
        collection.AddScoped<IOperationRepository, OperationRepository>();
        collection.AddScoped<ISessionRepository, SessionRepository>();

        return collection;
    }
}