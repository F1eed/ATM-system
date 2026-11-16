using System.Text.Json.Serialization;

namespace Service.Presentation.Http;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPresentationHttp(this IServiceCollection collection)
    {
        collection
            .AddControllers()
            .AddJsonOptions(options => options
                .JsonSerializerOptions
                .Converters.Add(new JsonStringEnumConverter()));

        return collection;
    }
}