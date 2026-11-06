using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;

namespace ConsoleClient.Presentation.Commands;

public class CheckBalanceCommand : AsyncCommand
{
    private readonly IBankService _bankService;
    private readonly ClientContext _clientContext;

    public CheckBalanceCommand(IBankService bankService, ClientContext clientContext)
    {
        _bankService = bankService;
        _clientContext = clientContext;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        CancellationToken cancellationToken)
    {
        if (_clientContext.UserSessionId is null)
        {
            AnsiConsole.MarkupLine("[red]ERROR: Authentication error[/]");
            return 1;
        }

        var request = new CheckBalance.Request(_clientContext.UserSessionId.Value);

        CheckBalance.Result result = await _bankService.CheckBalanceAsync(
            request,
            cancellationToken);

        if (result is CheckBalance.Result.Success success)
        {
            AnsiConsole.MarkupLine($"[green]Your balance: {success.Balance}[/]");
            return 0;
        }

        if (result is CheckBalance.Result.Failure failure)
        {
            AnsiConsole.MarkupLine($"[red]ERROR: {failure.ErrorMessage}[/]");
            return 1;
        }

        throw new UnreachableException();
    }
}