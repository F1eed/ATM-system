using Lab1.Application.Contracts.Account;
using Lab1.Application.Contracts.Session;
using Lab1.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lab1.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddScoped<ISessionService, SessionService>();
        collection.AddScoped<IAccountService, AccountService>();

        return collection;
    }
}