using Lab1.Application;
using Lab1.Infrastructure.Persistence;
using Lab1.Presentation.Http;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton<IConfiguration>(builder.Configuration)
    .AddApplication()
    .AddInfrastructurePersistence()
    .AddPresentationHttp();

builder.Services.AddSwaggerGen(
        x =>
        {
            x.UseOneOfForPolymorphism();
            x.UseInlineDefinitionsForEnums();
        })
    .AddEndpointsApiExplorer();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();