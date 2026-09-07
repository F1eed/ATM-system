using ConsoleClient.Application;
using ConsoleClient.Infrastructure.AccountService;
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
    .AddSessionClient()
    .AddAccountClient();

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

        create.AddCommand<CreateAccountCommand>("account");
    });

    config.AddCommand<DepositCommand>("deposit");

    config.AddCommand<WithdrawCommand>("withdraw");

    config.AddBranch("check", check =>
        {
            check.AddCommand<CheckBalanceCommand>("balance");
            check.AddCommand<CheckHistoryCommand>("history");
        });
});

while (true)
{
    string input = AnsiConsole.Ask<string>("[yellow]>[/]");

    if (input == "exit")
    {
        return;
    }

    string[] inputCommand = input.Split(' ');

    await app.RunAsync(inputCommand);
}
