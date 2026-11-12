using System.ComponentModel.DataAnnotations;

namespace ConsoleClient.Presentation.Options;

public class CliOptions
{
    [Range(1, int.MaxValue)]
    public int DefaultOperationsCount { get; set; }
}