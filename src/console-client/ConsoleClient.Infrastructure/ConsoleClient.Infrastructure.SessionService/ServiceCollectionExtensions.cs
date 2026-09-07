using ConsoleClient.Application.Abstractions.Session;
using ConsoleClient.Infrastructure.SessionService.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace ConsoleClient.Infrastructure.SessionService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSessionClient(this IServiceCollection collection)
    {
        collection
            .AddOptions<SessionClientOptions>()
            .BindConfiguration("Infrastructure:Configuration")
            .ValidateOnStart();

        collection
            .AddRefitClient<IRefitSessionClient>()
            .ConfigureHttpClient((provider, client) =>
            {
                IOptions<SessionClientOptions> options = provider
                    .GetRequiredService<IOptions<SessionClientOptions>>();
                client.BaseAddress = options.Value.Address;
            });

        collection.AddScoped<ISessionClient, SessionClient>();

        return collection;
    }
}