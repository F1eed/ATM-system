using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;

namespace ConsoleClient.Presentation.Commands;

public sealed class CreateAdminSessionCommand : AsyncCommand
{
    private readonly IBankService _service;
    private readonly ClientContext _clientContext;

    public CreateAdminSessionCommand(
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
        TextPrompt<string> password = new TextPrompt<string>("Enter password: ")
            .Secret();

        string passwordResult = AnsiConsole.Prompt(password);

        var request = new CreateAdminSession.Request(passwordResult);

        CreateAdminSession.Result result = await _service.CreateAdminSessionAsync(
            request,
            cancellationToken);

        if (result is CreateAdminSession.Result.Success success)
        {
            _clientContext.AdminSessionId = success.Session.SessionId;
            AnsiConsole.Console.MarkupLine(
                $"[green]Success create session with {success.Session.SessionId} ID.[/]");
            return 0;
        }

        if (result is CreateAdminSession.Result.Failure failure)
        {
            _clientContext.AdminSessionId = null;
            AnsiConsole.Console.MarkupLine($"[red]Error: {failure.ErrorMessage}[/]");
            return 1;
        }

        throw new UnreachableException();
    }
}