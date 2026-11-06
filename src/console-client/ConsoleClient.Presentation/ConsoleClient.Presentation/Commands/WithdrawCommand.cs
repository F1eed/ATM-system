using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Operations;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;

namespace ConsoleClient.Presentation.Commands;

public class WithdrawCommand : AsyncCommand
{
    private readonly IBankService _service;
    private readonly ClientContext _clientContext;

    public WithdrawCommand(
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
        if (_clientContext.UserSessionId is null)
        {
            AnsiConsole.MarkupLine("[red]ERROR: YOU ARE UNAUTHORIZED[/]");
            return 1;
        }

        var money = new TextPrompt<decimal>("Enter amount: ");

        decimal moneyResult = AnsiConsole.Prompt(money);

        var request = new Withdraw.Request(
            _clientContext.UserSessionId.Value,
            moneyResult);

        Withdraw.Result result = await _service.WithdrawAsync(
            request,
            cancellationToken);

        if (result is Withdraw.Result.Success success)
        {
            AnsiConsole.MarkupLine(
                $"[green]You success withdraw {moneyResult}. Current balance: {success.AccountDto.Balance}[/]");
            return 0;
        }

        if (result is Withdraw.Result.Failure failure)
        {
            AnsiConsole.MarkupLine($"[red]ERROR: {failure.ErrorMessage}[/]");
            return 1;
        }

        throw new UnreachableException();
    }
}