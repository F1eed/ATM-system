using Itmo.Dev.Platform.Common.Extensions;
using Service.Application;
using Service.Infrastructure.Persistence;
using Service.Presentation.Http;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddPlatform(config =>
    config.WithSystemTextJsonConfiguration());

builder.Services
    .AddApplication()
    .AddInfrastructurePersistence()
    .AddPresentationHttp();

WebApplication app = builder.Build();

app.MapControllers();

await app.RunAsync();