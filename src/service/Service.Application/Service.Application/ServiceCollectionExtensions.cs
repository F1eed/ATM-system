using Microsoft.Extensions.DependencyInjection;
using Service.Application.Contracts.Account;
using Service.Application.Contracts.Session;
using Service.Application.Services;
using Service.Application.Services.Options;

namespace Service.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection collection)
    {
        collection.AddOptions<SessionServiceOptions>()
            .BindConfiguration("Application:Sessions:SessionServiceOptions");

        collection.AddScoped<ISessionService, SessionService>();
        collection.AddScoped<IAccountService, AccountService>();

        return collection;
    }
}