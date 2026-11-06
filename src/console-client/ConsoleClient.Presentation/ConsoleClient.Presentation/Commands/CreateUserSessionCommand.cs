using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;

namespace ConsoleClient.Presentation.Commands;

public sealed class CreateUserSessionCommand : AsyncCommand
{
    private readonly IBankService _service;
    private readonly ClientContext _clientContext;

    public CreateUserSessionCommand(
        IBankService service,
        ClientContext clientContext)
    {
        _service = service;
        _clientContext = clientContext;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        CancellationToken cancellationToken)
    {
        TextPrompt<string> cardNumber = new TextPrompt<string>("Enter your card number: ")
            .Validate(input => input.Length == 16)
            .ValidationErrorMessage("Card number must be 16 characters");

        TextPrompt<string> pinCode = new TextPrompt<string>("Enter your [green]PIN[/]: ")
            .Secret('#')
            .Validate(input => input.Length == 4)
            .ValidationErrorMessage("PinCode must be 4 characters");

        string cardNumberResult = AnsiConsole.Prompt(cardNumber);

        string pinCodeResult = AnsiConsole.Prompt(pinCode);

        var request = new CreateUserSession.Request(
            cardNumberResult,
            pinCodeResult);

        CreateUserSession.Result result = await _service.CreateUserSessionAsync(
            request,
            cancellationToken);

        if (result is CreateUserSession.Result.Success success)
        {
            _clientContext.UserSessionId = success.Session.SessionId;
            AnsiConsole.Console.MarkupLine("[green]Success[/]");
            return 0;
        }

        if (result is CreateUserSession.Result.Failure failure)
        {
            _clientContext.UserSessionId = null;
            AnsiConsole.Console.MarkupLine($"[red]Error: {failure.ErrorMessage}[/]");
            return 1;
        }

        throw new UnreachableException();
    }
}