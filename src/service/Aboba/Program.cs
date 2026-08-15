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

builder.Services
    .AddSwaggerGen(x =>
    {
        x.UseOneOfForPolymorphism();
        x.UseInlineDefinitionsForEnums();
    })
    .AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

await app.RunAsync();