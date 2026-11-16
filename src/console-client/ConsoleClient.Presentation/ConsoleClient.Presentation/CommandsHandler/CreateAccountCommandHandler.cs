using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;

namespace ConsoleClient.Presentation.CommandsHandler;

public class CreateAccountCommandHandler : AsyncCommand
{
    private readonly ClientContext _clientContext;
    private readonly IBankService _service;

    public CreateAccountCommandHandler(IBankService service, ClientContext clientContext)
    {
        _service = service;
        _clientContext = clientContext;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        CancellationToken cancellationToken)
    {
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

        var request = new CreateAccount.Request(
            _clientContext.AdminSessionId.Value,
            entries);

        CreateAccount.Result result = await _service.CreateAccountAsync(
            request,
            cancellationToken);

        if (result is CreateAccount.Result.Failure failure)
        {
            AnsiConsole.MarkupLine($"[red]ERROR: {Markup.Escape(failure.ErrorMessage)}[/]");
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