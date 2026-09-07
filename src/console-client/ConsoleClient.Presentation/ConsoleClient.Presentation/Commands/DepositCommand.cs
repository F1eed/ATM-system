using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;

namespace ConsoleClient.Presentation.Commands;

public sealed class DepositCommand : AsyncCommand
{
    private readonly IBankService _service;
    private readonly ClientContext _clientContext;

    public DepositCommand(
        IBankService service,
        ClientContext clientContext)
    {
        _service = service;
        _clientContext = clientContext;
    }

    protected override async Task<int> ExecuteAsync(
        CommandContext context,
        CancellationToken cancellationToken)
    {
        if (_clientContext.UserSessionId is null)
        {
            AnsiConsole.MarkupLine("[red]ERROR: YOU ARE UNAUTHORIZED[/]");
            return 1;
        }

        var money = new TextPrompt<decimal>("Enter amount: ");

        decimal moneyResult = AnsiConsole.Prompt(money);

        var request = new Deposit.Request(
            _clientContext.UserSessionId.Value,
            moneyResult);

        Deposit.Result result = await _service.DepositAsync(
            request,
            cancellationToken);

        if (result is Deposit.Result.Success success)
        {
            AnsiConsole.MarkupLine(
                $"[green]You success deposit {moneyResult}. Current balance: {success.AccountDto.Balance}[/]");
            return 0;
        }

        if (result is Deposit.Result.Failure failure)
        {
            AnsiConsole.MarkupLine($"[red]ERROR: {failure.ErrorMessage}[/]");
            return 1;
        }

        throw new UnreachableException();
    }
}