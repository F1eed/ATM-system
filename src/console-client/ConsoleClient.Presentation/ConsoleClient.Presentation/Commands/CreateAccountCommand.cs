using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;

namespace ConsoleClient.Presentation.Commands;

public class CreateAccountCommand : AsyncCommand
{
    private readonly IBankService _service;
    private readonly ClientContext _clientContext;

    public CreateAccountCommand(IBankService service, ClientContext clientContext)
    {
        _service = service;
        _clientContext = clientContext;
    }

    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        CancellationToken cancellationToken)
    {
        // Сомнительно
        if (_clientContext.AdminSessionId is null)
        {
            AnsiConsole.MarkupLine("[red]Authentication Error: You must login to admin session[/]");
            return 1;
        }

        bool confirmed;
        string cardNumberResult;
        string pinCodeResult;
        var entries = new List<CreateAccount.AccountEntry>();

        do
        {
            var cardNumber = new TextPrompt<string>("Enter [blue]CardNumber[/]: ");
            var pinCode = new TextPrompt<string>("Enter [blue]PinCode[/]:");

            cardNumberResult = AnsiConsole.Prompt(cardNumber);
            pinCodeResult = AnsiConsole.Prompt(pinCode);

            confirmed = await AnsiConsole.ConfirmAsync(
                "Do you want to create another account?",
                true,
                cancellationToken);

            entries.Add(new CreateAccount.AccountEntry(
                cardNumberResult,
                pinCodeResult));
        }
        while (confirmed);

        // Норм у наллабл брать Value если проверил до этого на null и никак не менял?
        var request = new CreateAccount.Request(
            _clientContext.AdminSessionId.Value,
            entries);

        CreateAccount.Result result = await _service.CreateAccountAsync(
            request,
            cancellationToken);

        if (result is CreateAccount.Result.Failure failure)
        {
            AnsiConsole.MarkupLine($"""[red]Error: {failure.ErrorMessage}[/]""");
            return 1;
        }

        if (result is CreateAccount.Result.Success success)
        {
            AnsiConsole.MarkupLine("[green]Success create account with numbers:[/]");
            foreach (string entry in success.CardNumbers)
            {
                AnsiConsole.MarkupLine($"{entry}");
            }

            return 0;
        }

        throw new UnreachableException();
    }
}