using ConsoleClient.Application.Abstractions.Account;
using ConsoleClient.Infrastructure.AccountService.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace ConsoleClient.Infrastructure.AccountService;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAccountClient(this IServiceCollection collection)
    {
        collection
            .AddOptions<AccountClientOptions>()
            .BindConfiguration("Infrastructure:Configuration")
            .ValidateOnStart();

        collection
            .AddRefitClient<IRefitAccountClient>()
            .ConfigureHttpClient((provider, client) =>
            {
                IOptions<AccountClientOptions> options = provider
                    .GetRequiredService<IOptions<AccountClientOptions>>();
                client.BaseAddress = options.Value.Address;
            });

        collection.AddScoped<IAccountClient, AccountClient>();

        return collection;
    }
}
