using System.ComponentModel.DataAnnotations;

namespace ConsoleClient.Infrastructure.SessionService.Options;

public sealed class SessionClientOptions
{
    [MinLength(1)]
    public required Uri Address { get; init; }
}