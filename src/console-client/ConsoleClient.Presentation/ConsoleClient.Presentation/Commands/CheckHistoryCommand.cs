using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Models;
using ConsoleClient.Application.Contracts.Operations;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;
using System.Globalization;

namespace ConsoleClient.Presentation.Commands;

public class CheckHistoryCommand : AsyncCommand
{
    private readonly IBankService _bankService;
    private readonly ClientContext _clientContext;

    public CheckHistoryCommand(IBankService bankService, ClientContext clientContext)
    {
        _bankService = bankService;
        _clientContext = clientContext;
    }

    public override async Task<int> ExecuteAsync(CommandContext context, CancellationToken cancellationToken)
    {
        if (_clientContext.UserSessionId is null)
        {
            AnsiConsole.MarkupLine("[red]ERROR: Authentication error[/]");
            return 1;
        }

        var request = new CheckHistory.Request(_clientContext.UserSessionId.Value);

        CheckHistory.Result result = await _bankService.CheckHistoryAsync(
            request,
            cancellationToken);

        if (result is CheckHistory.Result.Success success)
        {
            Table table = new Table()
                .AddColumn("Occured at")
                .AddColumn("Operation type")
                .AddColumn("Amount")
                .AddColumn("Remaining Balance");

            foreach (OperationDto dto in success.OperationDtos)
            {
                table.AddRow(
                    dto.OccuredAt.ToLocalTime().ToString(
                        "yyyy.MM.dd HH:mm:ss tt",
                        CultureInfo.CurrentCulture),
                    dto.Type,
                    dto.Amount?.ToString(CultureInfo.InvariantCulture) ?? "—",
                    dto.Balance.ToString(CultureInfo.InvariantCulture));
            }

            AnsiConsole.Write(table);

            return 0;
        }

        if (result is CheckHistory.Result.Failure failure)
        {
            AnsiConsole.MarkupLine($"[red]ERROR: {failure.ErrorMessage}[/]");
            return 1;
        }

        throw new UnreachableException();
    }
}