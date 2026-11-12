using ConsoleClient.Application.Contracts;
using ConsoleClient.Application.Contracts.Models.Operations;
using ConsoleClient.Application.Contracts.Operations;
using ConsoleClient.Presentation.Commands;
using ConsoleClient.Presentation.Options;
using Microsoft.Extensions.Options;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics;
using System.Globalization;

namespace ConsoleClient.Presentation.CommandsHandler;

public class CheckHistoryCommandHandler : AsyncCommand<CheckHistoryCommand>
{
    private readonly IBankService _bankService;
    private readonly ClientContext _clientContext;
    private readonly IOptionsMonitor<CliOptions> _options;

    public CheckHistoryCommandHandler(
        IBankService bankService,
        ClientContext clientContext,
        IOptionsMonitor<CliOptions> options)
    {
        _bankService = bankService;
        _clientContext = clientContext;
        _options = options;
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        CheckHistoryCommand settings,
        CancellationToken cancellationToken)
    {
        if (_clientContext.UserSessionId is null)
        {
            AnsiConsole.MarkupLine("[red]ERROR: Authentication error[/]");
            return 1;
        }

        var request = new CheckHistory.Request(
            _clientContext.UserSessionId.Value,
            settings.OperationsCount ?? _options.CurrentValue.DefaultOperationsCount);

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

            // Играл с OCP -> Проиграл.
            foreach (BaseOperationDto dto in success.OperationDtos)
            {
                if (dto is DepositOperationDto deposit)
                {
                    table.AddRow(
                        deposit.OccuredAt.ToLocalTime()
                            .ToString(
                                "yyyy.MM.dd HH:mm:ss tt",
                                CultureInfo.CurrentCulture),
                        "Deposit",
                        deposit.Amount.ToString(CultureInfo.InvariantCulture),
                        deposit.RemainingBalance.ToString(CultureInfo.InvariantCulture));
                }

                if (dto is WithdrawOperationDto withdraw)
                {
                    table.AddRow(
                        withdraw.OccuredAt.ToLocalTime()
                            .ToString(
                                "yyyy.MM.dd HH:mm:ss tt",
                                CultureInfo.CurrentCulture),
                        "Withdraw",
                        withdraw.Amount.ToString(CultureInfo.InvariantCulture),
                        withdraw.RemainingBalance.ToString(CultureInfo.InvariantCulture));
                }

                if (dto is CheckBalanceOperationDto checkBalance)
                {
                    table.AddRow(
                        checkBalance.OccuredAt.ToLocalTime()
                            .ToString(
                                "yyyy.MM.dd HH:mm:ss tt",
                                CultureInfo.CurrentCulture),
                        "Check Balance",
                        "-",
                        checkBalance.Balance.ToString(CultureInfo.InvariantCulture));
                }
            }

            AnsiConsole.Write(table);

            return 0;
        }

        if (result is CheckHistory.Result.Failure failure)
        {
            AnsiConsole.MarkupLine($"[red]ERROR: {Markup.Escape(failure.ErrorMessage)}[/]");
            return 1;
        }

        throw new UnreachableException();
    }
}