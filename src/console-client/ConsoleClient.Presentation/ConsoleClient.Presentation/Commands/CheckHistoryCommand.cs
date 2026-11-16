using Spectre.Console;
using Spectre.Console.Cli;

namespace ConsoleClient.Presentation.Commands;

public sealed class CheckHistoryCommand : CommandSettings
{
    [CommandOption("--count")]
    public int? OperationsCount { get; init; }

    public override ValidationResult Validate()
    {
        return OperationsCount is null or > 0
            ? ValidationResult.Success()
            : ValidationResult.Error("invalid parameter --count(must be > 0)");
    }
}