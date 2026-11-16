using ConsoleClient.Application;
using ConsoleClient.Infrastructure.AccountService;
using ConsoleClient.Infrastructure.SessionService;
using ConsoleClient.Presentation;
using ConsoleClient.Presentation.CommandsHandler;
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
    .AddConsoleClientPresentation()
    .AddSessionClient()
    .AddAccountClient();

var registrar = new ServiceCollectionRegistrar(collection);

var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("Bank console client");

    config.AddBranch(
        "create",
        create =>
        {
            create.AddBranch(
                "admin",
                admin =>
                    admin.AddCommand<CreateAdminSessionCommandHandler>("session"));

            create.AddBranch(
                "user",
                user =>
                    user.AddCommand<CreateUserSessionCommandHandler>("session"));

            create.AddCommand<CreateAccountCommandHandler>("account");
        });

    config.AddCommand<DepositCommandHandler>("deposit");

    config.AddCommand<WithdrawCommandHandler>("withdraw");

    config.AddBranch(
        "check",
        check =>
        {
            check.AddCommand<CheckBalanceCommandHandler>("balance");
            check.AddCommand<CheckHistoryCommandHandler>("history");
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