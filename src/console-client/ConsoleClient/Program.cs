using ConsoleClient.Application;
using ConsoleClient.Infrastructure.SessionService;
using ConsoleClient.Presentation;
using ConsoleClient.Presentation.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

IConfigurationRoot configuration = new ConfigurationManager()
    .AddUserSecrets<Program>()
    .Build();

var clientContext = new ClientContext();

IServiceCollection collection = new ServiceCollection()
    .AddSingleton<IConfiguration>(configuration)
    .AddSingleton(clientContext)
    .AddApplication()
    .AddSessionClient();

var registrar = new ServiceCollectionRegistrar(collection);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("Bank console client");

    config.AddBranch("create", create =>
    {
        create.AddBranch("admin", admin =>
            admin.AddCommand<CreateAdminSessionCommand>("session"));

        create.AddBranch("user", user =>
                user.AddCommand<CreateUserSessionCommand>("session"));
    });
});

while (true)
{
    string input = AnsiConsole.Ask<string>(">");

    if (input == "exit")
    {
        return;
    }

    string[] inputCommand = input.Split(' ');

    await app.RunAsync(inputCommand);
}
