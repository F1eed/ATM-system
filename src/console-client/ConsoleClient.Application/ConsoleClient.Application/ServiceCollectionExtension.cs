using ConsoleClient.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleClient.Application;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddSingleton<IBankService, BankService>();

        return collection;
    }
}