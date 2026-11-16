using FluentMigrator.Runner;
using Scalar.AspNetCore;
using Service.Application;
using Service.Infrastructure.Persistence;
using Service.Presentation.Http;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructurePersistence(builder.Configuration)
    .AddPresentationHttp()
    .AddOpenApi(options => options.AddScalarTransformers());

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    IMigrationRunner runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
    runner.MigrateUp();
}

app.MapControllers();

app.MapOpenApi();
app.MapScalarApiReference();

await app.RunAsync();