using Lab1.Application.Abstractions.Persistence;
using Lab1.Application.Abstractions.Persistence.Repositories;
using Lab1.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Lab1.Infrastructure.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructurePersistence(this IServiceCollection collection)
    {
        collection.AddScoped<IPersistenceContext, PersistenceContext>();

        collection.AddSingleton<IAccountRepository, AccountRepository>();
        collection.AddSingleton<IOperationRepository, OperationRepository>();
        collection.AddSingleton<ISessionRepository, SessionRepository>();

        return collection;
    }
}