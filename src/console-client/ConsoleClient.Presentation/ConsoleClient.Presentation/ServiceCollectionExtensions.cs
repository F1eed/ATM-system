using ConsoleClient.Presentation.Options;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleClient.Presentation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleClientPresentation(this IServiceCollection collection)
    {
        collection
            .AddOptions<CliOptions>()
            .BindConfiguration("Presentation:CliOptions")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return collection;
    }
}