using ConsoleClient.Presentation.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleClient.Presentation;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPresentationConsole(this IServiceCollection collection)
    {
        collection.AddSingleton<ClientContext>();
        collection.AddSingleton<CreateAdminSessionCommand>();
        collection.AddSingleton<CreateUserSessionCommand>();

        return collection;
    }
}