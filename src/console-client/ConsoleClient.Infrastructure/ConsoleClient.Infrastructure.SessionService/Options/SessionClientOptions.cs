using System.ComponentModel.DataAnnotations;

namespace ConsoleClient.Infrastructure.SessionService.Options;

public sealed class SessionClientOptions
{
    [Required]
    public required Uri Address { get; init; }
}